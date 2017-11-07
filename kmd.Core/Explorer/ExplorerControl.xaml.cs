using kmd.Core.Command;
using kmd.Core.Explorer.Commands;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Controls;
using kmd.Core.Explorer.Controls.Breadcrumb;
using kmd.Core.ExplorerManager;
using kmd.Core.Helpers;
using kmd.Storage.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace kmd.Core.Explorer
{
    public sealed partial class ExplorerControl
    {
        public static readonly DependencyProperty CurrentFolderProperty =
            DependencyProperty.Register("CurrentFolder", typeof(StorageFolder), typeof(ExplorerControl), new PropertyMetadata(null));

        private ExplorerManagerControl _explorerManagerControl;
        private long _explorerManagerCurrentRegisteredCallback = 0;

        public ExplorerManagerControl ExplorerManagerControl
        {
            get
            {
                return _explorerManagerControl;
            }
            set
            {
                _explorerManagerControl?.UnregisterPropertyChangedCallback(ExplorerManagerControl.CurrentProperty, _explorerManagerCurrentRegisteredCallback);
                _explorerManagerCurrentRegisteredCallback = value.RegisterPropertyChangedCallback(ExplorerManagerControl.CurrentProperty, new DependencyPropertyChangedCallback((depOb, depProp) =>
                {
                    var control = (ExplorerControl)depOb?.GetValue(depProp);
                    if (control != null)
                    {
                        if (control.ExplorerId == this.ExplorerId)
                        {
                            IsActive = true;
                        }
                        else
                        {
                            IsActive = false;
                        }
                    }
                }));
                _explorerManagerControl = value;
            }
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public static readonly DependencyProperty IsActiveProperty =
            DependencyProperty.Register("IsActive", typeof(bool), typeof(ExplorerControl), new PropertyMetadata(false));

        public ExplorerControl()
        {
            InitializeComponent();

            SetBinding(CurrentFolderProperty, new Binding()
            {
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath("CurrentFolder"),
                Source = ViewModel
            });

            Loaded += ExplorerControl_Loaded;
            Unloaded += ExplorerControl_Unloaded;
        }

        public BreadcrumbControl BreadcrumbControl => Breadcrumb;

        public StorageFolder CurrentFolder
        {
            get => (StorageFolder)GetValue(CurrentFolderProperty);
            set => SetValue(CurrentFolderProperty, value);
        }

        public IStorageFolder RootFolder { get; set; }

        public int ExplorerId { get; set; }

        public PathBox PathBoxControl => PathBox;

        public ExplorerListView StorageItemsControl => StorageItems;

        public ExplorerViewModel ViewModel => RootElement.DataContext as ExplorerViewModel;

        private void Breadcrumb_ItemDragOver(object sender, BreadcrumbDragEventArgs e)
        {
            e.DragArgs.AcceptedOperation = (e.DragArgs.DataView.Contains(StandardDataFormats.StorageItems))
                ? DragOperations.UserRequestedDragOperation : DataPackageOperation.None;
        }

        private async void Breadcrumb_ItemDrop(object sender, BreadcrumbDragEventArgs e)
        {
            var droppedTarget = e.Item as IStorageFolder;
            if (droppedTarget == null) return;

            if (e.DragArgs.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var def = e.DragArgs.GetDeferral();
                var storageItems = await e.DragArgs.DataView.GetStorageItemsAsync();
                var changesMade = false;
                foreach (var item in storageItems)
                {
                    if (item is IStorageFolder)
                    {
                        var folder = await StorageFolder.GetFolderFromPathAsync(item.Path);
                        if (DragOperations.UserRequestedDragOperation == DataPackageOperation.Copy)
                        {
                            await folder.CopyContentsRecursiveAsync(droppedTarget, ViewModel.CancellationTokenSource.Token);
                        }
                        else if (DragOperations.UserRequestedDragOperation == DataPackageOperation.Move)
                        {
                            await folder.MoveContentsRecursiveAsync(droppedTarget, ViewModel.CancellationTokenSource.Token);
                        }
                        changesMade = true;
                    }
                    else if (item is IStorageFile)
                    {
                        var file = await StorageFile.GetFileFromPathAsync(item.Path);
                        if (DragOperations.UserRequestedDragOperation == DataPackageOperation.Copy)
                        {
                            await file.CopyAsync(droppedTarget, item.Name, NameCollisionOption.GenerateUniqueName);
                        }
                        else if (DragOperations.UserRequestedDragOperation == DataPackageOperation.Move)
                        {
                            await e.DragArgs.DataView.RequestAccessAsync();
                            await file.MoveAsync(droppedTarget, item.Name, NameCollisionOption.GenerateUniqueName);
                        }
                        changesMade = true;
                    }
                }

                if (changesMade)
                {
                    ViewModel.CurrentFolder = droppedTarget;
                }

                e.DragArgs.AcceptedOperation = DragOperations.UserRequestedDragOperation;
                def.Complete();
            }
        }

        private void Breadcrumb_ItemSelected(object sender, BreadcrumbEventArgs e)
        {
            if (e.Item is IStorageFolder folder)
            {
                ViewModel.CurrentFolder = folder;
            }
        }

        private async void ExplorerControl_Loaded(object sender, RoutedEventArgs e)
        {
            ExplorerManager.ExplorerManager.Register(this);
            await ViewModel.InitializeAsync(RootFolder);
        }

        private void ExplorerControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ExplorerManager.ExplorerManager.Unregister(this);
            _explorerManagerControl.UnregisterPropertyChangedCallback(ExplorerManagerControl.CurrentProperty, _explorerManagerCurrentRegisteredCallback);
        }

        private void StorageItems_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ViewModel.ExecuteCommand(typeof(OpenSelectedItemCommand));
        }

        private bool _isDragSource;

        private void StorageItems_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var storageItems = e.Items.Cast<IExplorerItem>()
                .Where(x => x.IsPhysical)
                .Select(x => x.StorageItem).ToList();
            if (storageItems.Any())
            {
                _isDragSource = true;
                e.Data.RequestedOperation = DragOperations.AvailableDragOperations;
                e.Data.SetStorageItems(storageItems);
            }
        }

        private void StorageItems_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            _isDragSource = false;
        }

        private void StorageItems_DragOver(object sender, DragEventArgs e)
        {
            var acceptedOperation = DataPackageOperation.None;

            if (e.DataView.Contains(StandardDataFormats.StorageItems) && !IsInsideTheSameExplorer())
            {
                acceptedOperation = DragOperations.UserRequestedDragOperation;
            }

            bool IsInsideTheSameExplorer()
            {
                return e.OriginalSource is DependencyObject dp && dp.FindParent<ExplorerControl>()._isDragSource;
            }

            e.AcceptedOperation = acceptedOperation;
        }

        private async void StorageItems_Drop(object sender, DragEventArgs e)
        {
            await AcceptDropAsync(e);
        }

        public async Task AcceptDropAsync(DragEventArgs e)
        {
            if (e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                var def = e.GetDeferral();
                var storageItems = await e.DataView.GetStorageItemsAsync();
                var changesMade = false;
                foreach (var item in storageItems)
                {
                    if (item is IStorageFolder)
                    {
                        var folder = await StorageFolder.GetFolderFromPathAsync(item.Path);
                        if (DragOperations.UserRequestedDragOperation == DataPackageOperation.Copy)
                        {
                            await folder.CopyContentsRecursiveAsync(CurrentFolder, ViewModel.CancellationTokenSource.Token);
                        }
                        else if (DragOperations.UserRequestedDragOperation == DataPackageOperation.Move)
                        {
                            await folder.MoveContentsRecursiveAsync(CurrentFolder, ViewModel.CancellationTokenSource.Token);
                        }
                        changesMade = true;
                    }
                    else if (item is IStorageFile)
                    {
                        var file = await StorageFile.GetFileFromPathAsync(item.Path);
                        if (DragOperations.UserRequestedDragOperation == DataPackageOperation.Copy)
                        {
                            await file.CopyAsync(CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                        }
                        else if (DragOperations.UserRequestedDragOperation == DataPackageOperation.Move)
                        {
                            await file.MoveAsync(CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                        }
                        changesMade = true;
                    }
                }

                if (changesMade)
                {
                    ViewModel.ExecuteCommand(typeof(NavigateCommand));
                }

                e.AcceptedOperation = DragOperations.UserRequestedDragOperation;
                def.Complete();
            }
        }

        private void StorageItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView?.SelectedItem != null)
            {
                StorageItemsControl.ForceFocusSelectedItem();
                listView.ScrollIntoView(listView.SelectedItem);
            }

            foreach (var item in e.AddedItems)
            {
                ViewModel.SelectedItems.Add(item as IExplorerItem);
            }

            foreach (var item in e.RemovedItems)
            {
                ViewModel.SelectedItems.Remove(item as IExplorerItem);
            }
        }

        private void AddNewFolder_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ExecuteCommand(typeof(AddNewFolderCommand));
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItems.Count < 2)
                ViewModel.SelectedItem = (sender as MenuFlyoutItem).DataContext as IExplorerItem;

            ViewModel.ExecuteCommand(typeof(CopySelectedItemCommand));
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItems.Count < 2)
                ViewModel.SelectedItem = (sender as MenuFlyoutItem).DataContext as IExplorerItem;

            ViewModel.ExecuteCommand(typeof(CutSelectedItemCommand));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedItems.Count < 2)
                ViewModel.SelectedItem = (sender as MenuFlyoutItem).DataContext as IExplorerItem;

            ViewModel.ExecuteCommand(typeof(DeleteSelectedItemCommand));
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ExecuteCommand(typeof(PasteToCurrentFolderCommand));
        }

        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = (sender as MenuFlyoutItem).DataContext as IExplorerItem;
            ViewModel.ExecuteCommand(typeof(ExplodeCurrentFolderCommand));
        }

        private void CopyPath_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = (sender as MenuFlyoutItem).DataContext as IExplorerItem;
            ViewModel.ExecuteCommand(typeof(ItemPathToClipboardCommand));
        }

        private void ChangeExtension_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = (sender as MenuFlyoutItem).DataContext as IExplorerItem;
            ViewModel.ExecuteCommand(typeof(ChangeExtensionCommand));
        }

        private void Rename_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = (sender as MenuFlyoutItem).DataContext as IExplorerItem;
            ViewModel.ExecuteCommand(typeof(RenameSelectedItemCommand));
        }

        private void GroupRename_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ExecuteCommand(typeof(GroupRenameCommand));
        }

        private void GroupItems_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ExecuteCommand(typeof(GroupItemsCommand));
        }

        private void Details_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedItem = (sender as MenuFlyoutItem).DataContext as IExplorerItem;
            ViewModel.ExecuteCommand(typeof(SelectedItemDetailsCommand));
        }

        private void Locations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var folder = (sender as ComboBox)?.SelectedItem as IStorageFolder;
            ViewModel.CurrentFolder = folder;
        }

        private async void PathBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var path = PathBox.Text;
            try
            {
                var storageFolder = await StorageFolder.GetFolderFromPathAsync(path);
                if (storageFolder != null)
                {
                    ViewModel.CurrentFolder = storageFolder;
                }
            }
            catch
            {
                PathBox.Text = ViewModel.CurrentFolder.Path;
            }
        }

        private void UserControl_GettingFocus(UIElement sender, Windows.UI.Xaml.Input.GettingFocusEventArgs args)
        {
            ExplorerManagerControl.Current = this;
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ExecuteCommand(typeof(OpenSelectedItemCommand));
        }

        private void OpenWith_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ExecuteCommand(typeof(OpenWithSelectedItemCommand));
        }
    }
}
