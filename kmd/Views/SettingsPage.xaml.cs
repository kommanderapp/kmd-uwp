using System;
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

        protected async override void OnNavigatedFrom(NavigationEventArgs e)
        {
            await ViewModel.SaveChangesAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ViewModel.EnsureInitialized();
        }

        private SettingsViewModel ViewModel => DataContext as SettingsViewModel;

        private async void Feedback_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault();
            await launcher.LaunchAsync();
        }
    }
}
