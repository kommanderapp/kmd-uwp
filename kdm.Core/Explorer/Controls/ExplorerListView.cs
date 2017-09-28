using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace kmd.Core.Explorer.Controls
{
    public class ExplorerListView : ListView
    {
        public bool IsFocusedEx = false;

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            IsFocusedEx = true;
            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            IsFocusedEx = false;
            base.OnGotFocus(e);
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            // Stop default behaviour for some keys
            if (e.Key == VirtualKey.Enter)
            {
                return;
            }

            base.OnKeyDown(e);
        }

        public void ForceFocusSelectedItem(FocusState state = FocusState.Keyboard)
        {
            // according to https://stackoverflow.com/questions/10444518/how-do-you-programmatically-set-focus-to-the-selecteditem-in-a-wpf-listbox-that
            // to properly set focus on listview item

            this.UpdateLayout();

            var listViewItem = (ListViewItem)this
                .ContainerFromItem(this.SelectedItem);
            if (listViewItem != null)
            {
                listViewItem.Focus(state);
            }
        }
    }
}