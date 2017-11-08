using kmd.Core.Explorer;
using kmd.Core.Hotkeys;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using kmd.Core.Helpers;
using kmd.Core.Command.Configuration;
using kmd.Core.ExplorerTabs.Commands;
using kmd.Core.ExplorerManager;
using System;
using System.Threading.Tasks;
using kmd.Core.Command;
using kmd.Core.Explorer.Commands;

namespace kmd.Core.ExplorerTabs
{
    public sealed partial class ExplorerTabsControl
    {
        public ExplorerManagerControl ExplorerManager { get; set; }
        public string ExplorerTabTag { get; set; }

        public ExplorerTabsControl()
        {
            InitializeComponent();
            ExplorerTabs.ItemsSource = Items;
            Loaded += ExplorerTabsControl_Loaded;
            Unloaded += ExplorerTabsControl_Unloaded;
            Items.CollectionChanged += Items_CollectionChanged;

            var explorerTabsCommands = CommandDescriptorProvider.GetCommandDescriptors().Where(x => x is ExplorerTabsCommandDescriptor);
            _addTabHotkey = explorerTabsCommands.FirstOrDefault(x => x.UniqueName == "AddTab").PreferredHotkey;
            _removeTabHotkey = explorerTabsCommands.FirstOrDefault(x => x.UniqueName == "RemoveTab").PreferredHotkey;
            _copyToOtherExplorerHotkey = explorerTabsCommands.FirstOrDefault(x => x.UniqueName == "CopyToOtherExplorer").PreferredHotkey;
            _moveToOtherExplorerHotkey = explorerTabsCommands.FirstOrDefault(x => x.UniqueName == "MoveToOtherExplorer").PreferredHotkey;
        }

        private async void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            await ApplicationData.Current.LocalSettings.SaveAsync<int?>($"{ExplorerTabTag}TabCount", Items.Count);
        }

        public ObservableCollection<object> Items { get; set; } = new ObservableCollection<object>();

        public void AddTab(StorageFolder storageFolder)
        {
            var explorer = new ExplorerControl();
            explorer.ExplorerTag = ExplorerTabTag + Items.Count;
            explorer.ExplorerManagerControl = ExplorerManager;
            explorer.RootFolder = storageFolder;

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

        private readonly Hotkey _addTabHotkey = null;
        private readonly Hotkey _removeTabHotkey = null;
        private readonly Hotkey _copyToOtherExplorerHotkey = null;
        private readonly Hotkey _moveToOtherExplorerHotkey = null;

        private async void ExplorerTabsControl_Loaded(object sender, RoutedEventArgs e)
        {
            KeyEventsAgregator.HotKey += HotkeyEventAgrigator_HotKey;
            var tabCount = await ApplicationData.Current.LocalSettings.ReadAsync<int?>($"{ExplorerTabTag}TabCount");

            if (tabCount == null)
                AddTab(null);
            else
            {
                for (int i = 0; i < tabCount; i++)
                {
                    var folder = await GetSavedLocation($"{ExplorerTabTag}{i}");
                    AddTab(folder);
                }
            }
        }
        private async Task<StorageFolder> GetSavedLocation(string explorerTag)
        {
            var path = await ApplicationData.Current.LocalSettings.ReadAsync<string>($"Explorer{explorerTag}SelectedLocation");
            if (path == null) return null;

            return await StorageFolder.GetFolderFromPathAsync(path);
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

        public ExplorerControl GetSelectedExplorerControl()
        {
            var selectedItem = (((PivotItem)ExplorerTabs.SelectedItem)?.Content as ExplorerControl);
            return selectedItem;
        }

        private void HotkeyEventAgrigator_HotKey(object sender, HotkeyEventArg e)
        {
            if (!IsTabControlInFocus) return;

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
            else if (e.Hotkey == _copyToOtherExplorerHotkey)
            {
                var selectedItem = GetSelectedExplorerControl();
                selectedItem.ViewModel.ExecuteCommand(typeof(CopySelectedItemCommand));

                var passiveTab = ExplorerManager.ExplorerTabsControl1.IsTabControlInFocus ? ExplorerManager.ExplorerTabsControl2 : ExplorerManager.ExplorerTabsControl1;
                var explorerToMoveAt = passiveTab.GetSelectedExplorerControl();

                explorerToMoveAt.ViewModel.ExecuteCommand(typeof(PasteToCurrentFolderCommand));

                e.Handled = true;
            }
            else if (e.Hotkey == _moveToOtherExplorerHotkey)
            {
                var selectedItem = GetSelectedExplorerControl();
                selectedItem.ViewModel.ExecuteCommand(typeof(CutSelectedItemCommand));

                var passiveTab = ExplorerManager.ExplorerTabsControl1.IsTabControlInFocus ? ExplorerManager.ExplorerTabsControl2 : ExplorerManager.ExplorerTabsControl1;
                var explorerToMoveAt = passiveTab.GetSelectedExplorerControl();

                explorerToMoveAt.ViewModel.ExecuteCommand(typeof(PasteToCurrentFolderCommand));

                e.Handled = true;
            }
        }

        public bool IsTabControlInFocus
        {
            get
            {
                var currentExplorer = ExplorerManager.Current;
                return Items.Cast<PivotItem>().Select(x => x.Content).Cast<ExplorerControl>().Any(x => x == currentExplorer);
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
