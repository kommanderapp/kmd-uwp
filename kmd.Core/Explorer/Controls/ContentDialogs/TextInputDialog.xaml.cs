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
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
    }
}
