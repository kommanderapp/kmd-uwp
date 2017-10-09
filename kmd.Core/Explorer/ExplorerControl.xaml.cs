using kmd.Core.Command;
using kmd.Core.Explorer.Commands;
using kmd.Core.Explorer.Controls;
using kmd.Core.Hotkeys;
using kmd.Helpers;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace kmd.Core.Explorer
{
    public sealed partial class ExplorerControl : UserControl
    {
        public static readonly DependencyProperty CurrentFolderProperty =
            DependencyProperty.Register("CurrentFolder", typeof(StorageFolder), typeof(ExplorerControl), new PropertyMetadata(null, CurrentFolder_Changed));

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

        public bool IsInFocus
        {
            get => StorageItems.IsFocusedEx;
        }

        public PathBox PathBoxControl => this.PathBox;
        public ExplorerListView StorageItemsControl => this.StorageItems as ExplorerListView;

        public ExplorerViewModel ViewModel
        {
            get { return RootElement.DataContext as ExplorerViewModel; }
        }

        private static void CurrentFolder_Changed(DependencyObject depObj, DependencyPropertyChangedEventArgs depProp)
        {
            //if (depObj is ExplorerControl explorer && depProp.NewValue != null)
            //{
            //    explorer.ViewModel.CurrentFolder = (IStorageFolder)depProp.NewValue;
            //}
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
            if (IsInFocus)
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
            if (!IsInFocus) return;

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