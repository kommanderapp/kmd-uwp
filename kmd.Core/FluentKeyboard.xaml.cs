using kmd.Core.Hotkeys;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace kmd.Core
{
    public sealed partial class FluentKeyboard : UserControl
    {
        public ModifierKeys ModifierKey
        {
            get { return (ModifierKeys)GetValue(ModifierKeyProperty); }
            set { SetValue(ModifierKeyProperty, value); }
        }

        public static readonly DependencyProperty ModifierKeyProperty =
            DependencyProperty.Register(nameof(ModifierKey), typeof(ModifierKeys), typeof(FluentKeyboard), new PropertyMetadata(ModifierKeys.None));

        public VirtualKey Key
        {
            get { return (VirtualKey)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register(nameof(Key), typeof(VirtualKey), typeof(FluentKeyboard), new PropertyMetadata(VirtualKey.None));

        public FluentKeyboard()
        {
            this.InitializeComponent();
        }
    }
}
