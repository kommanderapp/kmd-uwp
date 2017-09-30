using kmd.ViewModels;

using Windows.UI.Xaml.Controls;

namespace kmd.Views
{
    public sealed partial class ShellPage : Page
    {
        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame);
        }

        private ShellViewModel ViewModel
        {
            get { return DataContext as ShellViewModel; }
        }
    }
}
