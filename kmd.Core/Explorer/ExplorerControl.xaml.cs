using kmd.Core.Command;
using kmd.Core.Explorer.Commands;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Controls;
using kmd.Core.Explorer.Controls.Breadcrumb;
using kmd.Core.Hotkeys;
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
    public sealed partial class ExplorerControl : UserControl
    {
        public static readonly DependencyProperty CurrentFolderProperty =
            DependencyProperty.Register("CurrentFolder", typeof(StorageFolder), typeof(ExplorerControl), new PropertyMetadata(null));

        public ExplorerControl()
        {
            this.InitializeComponent();

            this.SetBinding(CurrentFolderProperty, new Binding()
            {
                Mode = BindingMode.TwoWay,
                Path = new PropertyPath("CurrentFolder"),
                Source = ViewModel
            });

            this.Loaded += ExplorerControl_Loaded;
            this.Unloaded += ExplorerControl_Unloaded;
        }

        public BreadcrumbControl BreadcrumbControl => this.Breadcrumb;

        public StorageFolder CurrentFolder
        {
            get { return (StorageFolder)GetValue(CurrentFolderProperty); }
            set { SetValue(CurrentFolderProperty, value); }
        }

        public int ExplorerId { get; set; }

        public bool ItemsInFocus
        {
            get => StorageItems.IsFocusedEx;
        }

        public PathBox PathBoxControl => this.PathBox;

        public ExplorerListView StorageItemsControl => this.StorageItems as ExplorerListView;

        public ExplorerViewModel ViewModel
        {
            get { return RootElement.DataContext as ExplorerViewModel; }
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
                        await (item as IStorageFolder).CopyContentsRecursiveAsync(CurrentFolder, ViewModel.CancellationTokenSource.Token);
                        changesMade = true;
                    }
                    else if (item is IStorageFile)
                    {
                        await (item as IStorageFile).CopyAsync(CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                        changesMade = true;
                    }
                }

                if (changesMade)
                {
                    this.ViewModel.ExecuteCommand(typeof(NavigateCommand));
                }

                e.AcceptedOperation = DataPackageOperation.Copy;
                def.Complete();
            }
        }

        private void Breadcrumb_ItemDragOver(object sender, BreadcrumbDragEventArgs e)
        {
            e.DragArgs.AcceptedOperation = (e.DragArgs.DataView.Contains(StandardDataFormats.StorageItems))
                ? DataPackageOperation.Copy : DataPackageOperation.None;
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
                        await (item as IStorageFolder).CopyContentsRecursiveAsync(droppedTarget, ViewModel.CancellationTokenSource.Token);
                        changesMade = true;
                    }
                    else if (item is IStorageFile)
                    {
                        await (item as IStorageFile).CopyAsync(droppedTarget, item.Name, NameCollisionOption.GenerateUniqueName);
                        changesMade = true;
                    }
                }

                if (changesMade)
                {
                    this.ViewModel.CurrentFolder = droppedTarget;
                }

                e.DragArgs.AcceptedOperation = DataPackageOperation.Copy;
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

        private void CharacterRecieved(object sender, CharReceivedEventArgs args)
        {
            if (!ItemsInFocus) return;
            if (!string.IsNullOrEmpty(args.Character))
            {
                ViewModel.LastTypedChar = args.Character;
                args.Handled = true;
            }
        }

        private void ExplorerControl_Loaded(object sender, RoutedEventArgs e)
        {
            ExplorerManager.Register(this);
            KeyEventsAgregator.HotKey += HotKeyPressed;
            KeyEventsAgregator.CharacterReceived += CharacterRecieved;
        }

        private void ExplorerControl_Unloaded(object sender, RoutedEventArgs e)
        {
            ExplorerManager.Unregister(this);
            KeyEventsAgregator.HotKey -= HotKeyPressed;
            KeyEventsAgregator.CharacterReceived -= CharacterRecieved;
        }

        private void HotKeyPressed(object sender, HotkeyEventArg e)
        {
            if (!ItemsInFocus) return;

            var command = ViewModel.CommandBindings.OfHotkey(e.Hotkey);
            if (command != null)
            {
                ViewModel.ExecuteCommand(command.GetType());
                e.Handled = true;
            }
        }

        private void StorageItems_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ViewModel.ExecuteCommand(typeof(OpenSelectedItemCommand));
        }

        private void StorageItems_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            var storageItems = e.Items.Cast<IExplorerItem>()
                .Where(x => x.IsPhysical)
                .Select(x => x.StorageItem);
            if (storageItems.Any())
            {
                e.Data.SetStorageItems(storageItems);
                e.Data.RequestedOperation = DataPackageOperation.Copy;
            }
        }

        private void StorageItems_DragOver(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = (e.DataView.Contains(StandardDataFormats.StorageItems)) ? DataPackageOperation.Copy : DataPackageOperation.None;
        }

        private async void StorageItems_Drop(object sender, DragEventArgs e)
        {
            await AcceptDropAsync(e);
        }

        private void StorageItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var listView = sender as ListView;
            if (listView.SelectedItem != null)
            {
                StorageItemsControl.ForceFocusSelectedItem();
                listView.ScrollIntoView(listView.SelectedItem);
            }
        }
    }
}