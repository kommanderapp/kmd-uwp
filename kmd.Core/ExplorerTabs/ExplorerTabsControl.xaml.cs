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
using kmd.Core.Helpers;

namespace kmd.Core.ExplorerTabs
{
    public sealed partial class ExplorerTabsControl
    {
        public ExplorerTabsControl()
        {
            InitializeComponent();
            ExplorerTabs.ItemsSource = Items;
            Loaded += ExplorerTabsControl_Loaded;
            Unloaded += ExplorerTabsControl_Unloaded;
        }

        public bool IsInFocus
        {
            get
            {
                if (ExplorerTabs.SelectedItem is PivotItem pivotItem)
                {
                    var explorerControl = pivotItem.Content as ExplorerControl;
                    return explorerControl != null && (explorerControl.ItemsInFocus);
                }
                return false;
            }
        }

        public ObservableCollection<object> Items { get; set; } = new ObservableCollection<object>();

        public void AddTab(StorageFolder storageFolder)
        {
            var explorer = new ExplorerControl();
            if (storageFolder != null)
            {
                explorer.Loaded += (s, e) => { (s as ExplorerControl).CurrentFolder = storageFolder; };
            }

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

            var pvHeader = new ExplorerTabHeader
            {
                AllowDrop = true
            };
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

        private readonly Hotkey _addTabHotkey = Hotkey.For(ModifierKeys.Control, VirtualKey.T);

        private readonly Hotkey _removeTabHotkey = Hotkey.For(ModifierKeys.Control, VirtualKey.W);

        private void ExplorerTabsControl_Loaded(object sender, RoutedEventArgs e)
        {
            KeyEventsAgregator.HotKey += HotkeyEventAgrigator_HotKey;
            this.AddTab(null);
        }

        private void ExplorerTabsControl_Unloaded(object sender, RoutedEventArgs e)
        {
            KeyEventsAgregator.HotKey -= HotkeyEventAgrigator_HotKey;
        }

        private void ForceFocusSelectedExplorer()
        {
            var explorer = GetSelectedExplorerControl();
            explorer?.StorageItemsControl.Focus(FocusState.Programmatic);
        }

        private ExplorerControl GetSelectedExplorerControl()
        {
            var selectedItem = (((PivotItem)ExplorerTabs.SelectedItem)?.Content as ExplorerControl);
            return selectedItem;
        }

        private void HotkeyEventAgrigator_HotKey(object sender, HotkeyEventArg e)
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
                e.AcceptedOperation = DragOperations.UserRequestedDragOperation;
                var selectedItem = Items.Cast<PivotItem>().FirstOrDefault(x => x.Header == explorerTabHeader);
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
                var selectedItem = Items.Cast<PivotItem>().FirstOrDefault(x => x.Header == explorerTabHeader);
                if (selectedItem?.Content is ExplorerControl explorer)
                {
                    await explorer.AcceptDropAsync(e);
                }
            }
        }
    }
}
