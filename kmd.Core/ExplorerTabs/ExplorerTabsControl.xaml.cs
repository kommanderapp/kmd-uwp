using kmd.Core.Explorer;
using kmd.Core.Hotkeys;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace kmd.Core.ExplorerTabs
{
    public sealed partial class ExplorerTabsControl : UserControl
    {
        public static readonly DependencyProperty RootFolderProperty =
                    DependencyProperty.Register("RootFolder", typeof(StorageFolder), typeof(ExplorerTabsControl), new PropertyMetadata(null, RootFolder_Changed));

        public ExplorerTabsControl()
        {
            this.InitializeComponent();
            this.ExplorerTabs.ItemsSource = Items;
            this.Loaded += ExplorerTabsControl_Loaded;
            this.Unloaded += ExplorerTabsControl_Unloaded;
        }

        public bool IsInFocus
        {
            get => ((this.ExplorerTabs.SelectedItem as PivotItem).Content as ExplorerControl).ItemsInFocus;
        }

        public ObservableCollection<object> Items { get; set; } = new ObservableCollection<object>();

        public StorageFolder RootFolder
        {
            get { return (StorageFolder)GetValue(RootFolderProperty); }
            set { SetValue(RootFolderProperty, value); }
        }

        public void AddTab(StorageFolder storageFolder)
        {
            var explorer = new ExplorerControl();
            explorer.CurrentFolder = storageFolder;

            var pvItem = ConfigurePivotItem(explorer);

            Items.Add(pvItem);
            ExplorerTabs.SelectedIndex = Items.Count - 1;
            ForceFocusSelectedExplorer();
        }

        private PivotItem ConfigurePivotItem(ExplorerControl explorer)
        {
            var pvItem = new PivotItem()
            {
                Content = explorer
            };

            var pvHeader = new ExplorerTabHeader();

            pvHeader.AllowDrop = true;
            pvHeader.Drop += PivotHeaderItem_OnDrop;
            pvHeader.DragOver += PivotHeaderItem_OnDragOver;
            pvItem.Header = pvHeader;

            pvHeader.SetBinding(ExplorerTabHeader.LabelTextProperty, new Binding()
            {
                Path = new PropertyPath("CurrentFolder.DisplayName"),
                Source = explorer,
                Mode = BindingMode.OneWay
            });
            return pvItem;
        }

        public void RemoveTab(int index)
        {
            if (Items.Count > 1)
            {
                Items.RemoveAt(index);
                ExplorerTabs.SelectedIndex = index != 0 ? index - 1 : index + 1;
                ForceFocusSelectedExplorer();
            }
        }

        private Hotkey _addTabHotkey = Hotkey.For(ModifierKeys.Control, VirtualKey.T);

        private Hotkey _removeTabHotkey = Hotkey.For(ModifierKeys.Control, VirtualKey.W);

        private static void RootFolder_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ExplorerTabsControl;
            if (control.Items.Count == 0)
            {
                control.AddTab((StorageFolder)e.NewValue);
            }
        }

        private void ExplorerTabsControl_Loaded(object sender, RoutedEventArgs e)
        {
            KeyEventsAgregator.HotKey += HotkeyEventAgrigator_HotKey;
        }

        private void ExplorerTabsControl_Unloaded(object sender, RoutedEventArgs e)
        {
            KeyEventsAgregator.HotKey -= HotkeyEventAgrigator_HotKey;
        }

        private void ForceFocusSelectedExplorer()
        {
            var explorer = GetSelectedExplorerControl();
            if (explorer != null)
            {
                explorer.StorageItemsControl.Focus(FocusState.Programmatic);
            }
        }

        private ExplorerControl GetSelectedExplorerControl()
        {
            var selectedItem = (((PivotItem)ExplorerTabs.SelectedItem)?.Content as ExplorerControl);
            return selectedItem;
        }

        private void HotkeyEventAgrigator_HotKey(object sender, Hotkeys.HotkeyEventArg e)
        {
            if (!IsInFocus) return;

            if (e.Hotkey == _addTabHotkey)
            {
                var selectedItem = GetSelectedExplorerControl();
                if (selectedItem != null)
                {
                    var currentFolder = selectedItem.ViewModel.CurrentFolder;
                    AddTab((StorageFolder)currentFolder);
                    e.Handled = true;
                }
            }
            else if (e.Hotkey == _removeTabHotkey)
            {
                var selectedIndex = ExplorerTabs.SelectedIndex;
                RemoveTab(selectedIndex);
                e.Handled = true;
            }
        }

        private void PivotHeaderItem_OnDragOver(object sender, DragEventArgs e)
        {
            if (sender is ExplorerTabHeader explorerTabHeader && e.DataView.Contains(StandardDataFormats.StorageItems))
            {
                e.AcceptedOperation = DataPackageOperation.Copy;
                var selectedItem = Items.Cast<PivotItem>().Where(x => x.Header == explorerTabHeader).FirstOrDefault();
                ExplorerTabs.SelectedItem = selectedItem;
            }
            else
            {
                e.AcceptedOperation = DataPackageOperation.None;
            }
        }

        private async void PivotHeaderItem_OnDrop(object sender, DragEventArgs e)
        {
            if (sender is ExplorerTabHeader explorerTabHeader)
            {
                var selectedItem = Items.Cast<PivotItem>().Where(x => x.Header == explorerTabHeader).FirstOrDefault();
                if (selectedItem.Content is ExplorerControl explorer)
                {
                    await explorer.AcceptDropAsync(e);
                }
            }
        }

    }
}