using kmd.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace kmd.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.Initialize();
        }

        private SettingsViewModel ViewModel
        {
            get { return DataContext as SettingsViewModel; }
        }

        private async void Feedback_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            launcher.LaunchAsync();
        }
    }
}
