using kmd.Core.Command;
using kmd.Core.Explorer.Commands;
using kmd.Core.Explorer.Controls;
using kmd.Core.Hotkeys;
using kmd.Helpers;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace kmd.Core.Explorer
{
    public sealed partial class ExplorerControl : UserControl
    {
        public static readonly DependencyProperty RootFolderProperty =
            DependencyProperty.Register("RootFolder", typeof(StorageFolder), typeof(ExplorerControl), new PropertyMetadata(null, RootFolder_Changed));

        public ExplorerControl()
        {
            this.InitializeComponent();

            this.Loaded += ExplorerControl_Loaded;
            this.Unloaded += ExplorerControl_Unloaded;
        }

        public BreadcrumbControl BreadcrumbControl => this.Breadcrumb;
        public PathBox PathBoxControl => this.PathBox;

        public StorageFolder RootFolder
        {
            get { return (StorageFolder)GetValue(RootFolderProperty); }
            set { SetValue(RootFolderProperty, value); }
        }

        public ExplorerListView StorageItemsControl => this.StorageItems as ExplorerListView;

        public ExplorerViewModel ViewModel
        {
            get { return RootElement.DataContext as ExplorerViewModel; }
        }

        private static void RootFolder_Changed(DependencyObject depObj, DependencyPropertyChangedEventArgs depProp)
        {
            if (depObj is ExplorerControl explorer && depProp.NewValue != null)
            {
                explorer.ViewModel.CurrentFolder = (IStorageFolder)depProp.NewValue;
            }
        }

        private void Breadcrumb_ItemSelected(object sender, BreadcrumbEventArgs e)
        {
            var folder = e.Item as IStorageFolder;
            if (folder != null)
            {
                ViewModel.CurrentFolder = folder;
            }
        }

        private void CharacterRecieved(object sender, CharReceivedEventArgs args)
        {
            if (StorageItemsControl.IsFocusedEx)
            {
                if (!string.IsNullOrEmpty(args.Character))
                {
                    ViewModel.LastTypedChar = args.Character;
                    args.Handled = true;
                }
            }
        }

        private void ExplorerControl_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO ExplorerManager
            KeyEventsAgregator.HotKey += HotKeyPressed;
            KeyEventsAgregator.CharacterReceived += CharacterRecieved;
        }

        private void ExplorerControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // TODO ExplorerManager
            KeyEventsAgregator.HotKey -= HotKeyPressed;
            KeyEventsAgregator.CharacterReceived -= CharacterRecieved;
        }

        private void HotKeyPressed(object sender, HotkeyEventArg e)
        {
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