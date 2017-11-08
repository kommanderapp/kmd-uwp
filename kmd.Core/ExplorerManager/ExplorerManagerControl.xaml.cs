using kmd.Core.Command;
using kmd.Core.Explorer;
using kmd.Core.Explorer.Commands;
using kmd.Core.Explorer.States;
using kmd.Core.Hotkeys;
using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace kmd.Core.ExplorerManager
{
    public sealed partial class ExplorerManagerControl : UserControl
    {
        public ExplorerManagerControl()
        {
            this.InitializeComponent();
            Explorer1.ExplorerManager = this;
            Explorer2.ExplorerManager = this;
            this.Loaded += ExplorerManager_Loaded;
            this.Unloaded += ExplorerManager_Unloaded;
        }

        private void ExplorerManager_Unloaded(object sender, RoutedEventArgs e)
        {
            KeyEventsAgregator.HotKey -= HotKeyPressed;
            KeyEventsAgregator.CharacterReceived -= CharacterRecieved;
        }

        private void ExplorerManager_Loaded(object sender, RoutedEventArgs e)
        {
            KeyEventsAgregator.HotKey += HotKeyPressed;
            KeyEventsAgregator.CharacterReceived += CharacterRecieved;
        }

        private void CharacterRecieved(object sender, CharReceivedEventArgs args)
        {
            if (Current == null) return;

            if (!string.IsNullOrEmpty(args.Character))
            {
                Current.ViewModel.LastTypedChar = args.Character;
                args.Handled = true;
            }
        }

        private void HotKeyPressed(object sender, HotkeyEventArg e)
        {
            if (Current == null) return;
            var command = Current.ViewModel.CommandBindings.OfHotkey(e.Hotkey);
            if (command != null)
            {
                Current.ViewModel.ExecuteCommand(command.GetType());
                e.Handled = true;
            }
        }

        private void NavigateBackward_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(NavigateBackwardCommand));
        }

        private void NavigateForward_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(NavigateForwardCommand));
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(CopySelectedItemCommand));
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(CutSelectedItemCommand));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(DeleteSelectedItemCommand));
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(PasteToCurrentFolderCommand));
        }

        private void Expand_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(ExplodeCurrentFolderCommand));
        }

        private void CopyPath_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(ItemPathToClipboardCommand));
        }

        private void NavigateUp_Click(object sender, RoutedEventArgs e)
        {
            Current?.ViewModel?.ExecuteCommand(typeof(NavigateToParrentCommand));
        }

        private void SortMethod_Changed(object sender, RoutedEventArgs e)
        {
            if (Current?.ViewModel?.ExplorerItems == null) return;
            var item = (RadioButton)sender;

            var method = Enum.GetValues(typeof(SortMethod)).Cast<SortMethod>().First(o => o.ToString().Equals(item.Name));
            Current.ViewModel.Sort(method);
        }

        public ExplorerControl Current
        {
            get { return (ExplorerControl)GetValue(CurrentProperty); }
            set { SetValue(CurrentProperty, value); }
        }

        public static readonly DependencyProperty CurrentProperty =
            DependencyProperty.Register("Current", typeof(ExplorerControl), typeof(ExplorerManagerControl), new PropertyMetadata(null, CurrentExplorerChanged));

        private static void CurrentExplorerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ExplorerManagerControl explorerManager) explorerManager.UpdateBindings(); 
        }

        public bool ChangeExplorerViewState
        {
            get
            {
                if (Current == null) return false;

                switch (Current.ExplorerViewStates)
                {
                    case ExplorerViewStates.DataGrid:
                        return false;                        
                    case ExplorerViewStates.Tiles:
                        return true;
                    default:
                        return false;
                }
            }
            set
            {
                if (Current != null)
                {
                    if (value)
                    {
                        Current.ExplorerViewStates = ExplorerViewStates.Tiles;
                    }
                    else
                    {
                        Current.ExplorerViewStates = ExplorerViewStates.DataGrid;
                    }
                }

                Bindings.Update();
            }
        }

        public void UpdateBindings()
        {
            Bindings.Update();
        }

        private string _searchQueryText;
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if(args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                _searchQueryText = sender.Text;
            }
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {           
            Current.ViewModel.FilterOptions = new Storage.Contracts.FilterOptions() { QueryText = _searchQueryText };
        }

        private void AutoSuggestBox_GotFocus(object sender, RoutedEventArgs e)
        {
            KeyEventsAgregator.IsDisabled = true;
        }

        private void AutoSuggestBox_LostFocus(object sender, RoutedEventArgs e)
        {
            KeyEventsAgregator.IsDisabled = false;
        }
    }
}
