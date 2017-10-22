using kmd.Core.Explorer;
using kmd.Core.Hotkeys;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

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

            var pvItem = new PivotItem()
            {
                Content = explorer
            };

            pvItem.SetBinding(PivotItem.HeaderProperty, new Binding()
            {
                Path = new PropertyPath("CurrentFolder.DisplayName"),
                Source = explorer,
                Mode = BindingMode.OneWay
            });

            Items.Add(pvItem);
            ExplorerTabs.SelectedIndex = Items.Count - 1;
            ForceFocusSelectedExplorer();
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
    }
}