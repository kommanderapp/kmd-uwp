using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace kmd.Core.Explorer.Controls
{
    public class PathBox : TextBox
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EscFallbackValueProperty =
            DependencyProperty.Register("EscFallbackValue", typeof(string), typeof(PathBox), new PropertyMetadata(0));

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FocusFallbackControlProperty =
            DependencyProperty.Register("FocusFallbackControl", typeof(Control), typeof(PathBox), new PropertyMetadata(0));

        public string EscFallbackValue
        {
            get { return (string)GetValue(EscFallbackValueProperty); }
            set { SetValue(EscFallbackValueProperty, value); }
        }

        public Control FocusFallbackControl
        {
            get { return (Control)GetValue(FocusFallbackControlProperty); }
            set { SetValue(FocusFallbackControlProperty, value); }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);

            this.Select(0, this.Text.Length);
        }

        protected override void OnKeyDown(KeyRoutedEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.Key == VirtualKey.Escape)
            {
                this.Text = EscFallbackValue;
                e.Handled = true;
                FocusFallbackControl?.Focus(FocusState.Keyboard);
            }
            if (e.Key == VirtualKey.Enter)
            {
                e.Handled = true;
                FocusFallbackControl?.Focus(FocusState.Keyboard);
            }
        }
    }
}