using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace kmd.Core.ExplorerTabs
{
    public sealed partial class ExplorerTabHeader : UserControl
    {
        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty);
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly DependencyProperty LabelTextProperty =
            DependencyProperty.Register("LabelText", typeof(string), typeof(ExplorerTabHeader), new PropertyMetadata(string.Empty));

        public ExplorerTabHeader()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }
    }
}