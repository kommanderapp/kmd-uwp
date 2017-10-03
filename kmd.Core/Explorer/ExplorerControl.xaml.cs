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

            _keyEventsAgregator = new KeyEventsAgregator();

            RegisterHotkeyHandlers();
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

        private KeyEventsAgregator _keyEventsAgregator;

        private static void RootFolder_Changed(DependencyObject depObj, DependencyPropertyChangedEventArgs depProp)
        {
            if (depObj is ExplorerControl explorer && depProp.NewValue != null)
            {
                explorer.ViewModel.CurrentFolder = (IStorageFolder)depProp.NewValue;
            }
        }

        private void CoreWindow_CharacterRecieved(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            if (StorageItemsControl.IsFocusedEx)
            {
                args.Handled = true;
                ViewModel.LastTypedChar = Unicode.ToString(args.KeyCode);
            }
        }

        private void ExplorerControl_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO ExplorerManager
        }

        private void ExplorerControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // TODO ExplorerManager
        }

        private void HotKeyPressed(object sender, HotkeyEventArg e)
        {
            //var command = ViewModel.CommandBindings.OfHotkey(e.Hotkey);
            //if (command != null)
            //{
            //    command.Execute(null);
            //    e.Handled = true;
            //}
        }

        private void PathBox_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        private void RegisterHotkeyHandlers()
        {
            if (Window.Current?.CoreWindow != null)
            {
                Window.Current.CoreWindow.CharacterReceived += CoreWindow_CharacterRecieved;
            }

            RootElement.KeyUp += _keyEventsAgregator.KeyUpHandler;
            RootElement.KeyDown += _keyEventsAgregator.KeyDownHandler;
            _keyEventsAgregator.HotKey += HotKeyPressed;
        }

        private void StorageItems_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ViewModel.OpenSelectedItemCommand.Execute(ViewModel);
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