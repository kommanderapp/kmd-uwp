using kdm.Core.Explorer.Commands.Default;
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
        private KeyEventsAgregator _keyEventsAgregator;

        public ExplorerControl()
        {
            this.InitializeComponent();

            this.Loaded += ExplorerControl_Loaded;
            this.Unloaded += ExplorerControl_Unloaded;

            _keyEventsAgregator = new KeyEventsAgregator();

            RegisterHotkeyHandlers();
        }

        private void ExplorerControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // TODO ExplorerManager
        }

        private void ExplorerControl_Loaded(object sender, RoutedEventArgs e)
        {
            // TODO ExplorerManager
        }

        public ExplorerViewModel ViewModel
        {
            get { return RootElement.DataContext as ExplorerViewModel; }
        }

        public PathBox PathBoxControl => this.PathBox;
        public BreadcrumbControl BreadcrumbControl => this.Breadcrumb;
        public ExplorerListView StorageItemsControl => this.StorageItems as ExplorerListView;

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

        private void HotKeyPressed(object sender, HotkeyEventArg e)
        {
            var command = ViewModel.CommandBindings.GetCommandByHotkey(e.Hotkey);
            if (command != null)
            {
                command.Execute(ViewModel.Model);
                e.Handled = true;
            }
        }

        public static readonly DependencyProperty RootFolderProperty =
            DependencyProperty.Register("RootFolder", typeof(StorageFolder), typeof(ExplorerControl), new PropertyMetadata(null, RootFolder_Changed));

        public StorageFolder RootFolder
        {
            get { return (StorageFolder)GetValue(RootFolderProperty); }
            set { SetValue(RootFolderProperty, value); }
        }

        private void CoreWindow_CharacterRecieved(CoreWindow sender, CharacterReceivedEventArgs args)
        {
            if (StorageItemsControl.IsFocusedEx)
            {
                args.Handled = true;
                ViewModel.CommandBindings[nameof(TypingHiglightCommand)].Execute(Unicode.ToString(args.KeyCode));
            }
        }

        private static void RootFolder_Changed(DependencyObject depObj, DependencyPropertyChangedEventArgs depProp)
        {
            if (depObj is ExplorerControl explorer && depProp.NewValue != null)
            {
                explorer.ViewModel.CommandBindings[nameof(NavigateCommand)].Execute((IStorageFolder)depProp.NewValue);
            }
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

        private void StorageItems_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            ViewModel.CommandBindings[nameof(OpenSelectedItemCommand)].Execute(ViewModel);
        }

        private void PathBox_LostFocus(object sender, RoutedEventArgs e)
        {
        }
    }
}