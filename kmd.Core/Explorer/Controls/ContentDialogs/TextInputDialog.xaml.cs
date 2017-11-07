using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace kmd.Core.Explorer.Controls.ContentDialogs
{
    public sealed partial class TextInputDialog : ContentDialog
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
             "Text", typeof(string), typeof(TextInputDialog), new PropertyMetadata(string.Empty));

        public TextInputDialog()
        {
            InitializeComponent();

            CloseButtonText = "Cancel";
            
            textBox.TextChanged += TextBox_TextChanged;
            textBox.KeyDown += TextBox_KeyDown;
        }

        private void TextBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                Hide();
        }

        private bool _initialized = false;
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!_initialized)
            {
                textBox.SelectAll();
                _initialized = true;
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
