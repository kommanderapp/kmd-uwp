using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace kmd.Core.Behaviors
{
    public class FocusBehavior : Behavior<Control>
    {
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached("IsFocused", typeof(bool), typeof(FocusBehavior), new PropertyMetadata(false, IsFocusedChanged));

        public bool IsFocused
        {
            get { return (bool)GetValue(IsFocusedProperty); }
            set { SetValue(IsFocusedProperty, value); }
        }

        protected override void OnAttached()
        {
            AssociatedObject.GotFocus += Control_GotFocus;
            AssociatedObject.LostFocus += Control_LostFocus;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.GotFocus -= Control_GotFocus;
            AssociatedObject.LostFocus -= Control_LostFocus;
        }

        private static void IsFocusedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var cntrl = ((FocusBehavior)d).AssociatedObject;
            if ((bool)e.NewValue)
            {
                cntrl.Focus(FocusState.Keyboard);
            }
        }

        private void Control_GotFocus(object sender, RoutedEventArgs e)
        {
            IsFocused = true;
        }

        private void Control_LostFocus(object sender, RoutedEventArgs e)
        {
            IsFocused = false;
        }
    }
}