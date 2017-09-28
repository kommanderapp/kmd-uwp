using kmd.Services;
using kmd.Views;

using Microsoft.Practices.ServiceLocation;

namespace kmd.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ConfigureNavigationService();
        }

        private void ConfigureNavigationService()
        {
            NavigationService.Configure(typeof(MainViewModel).FullName, typeof(MainPage));
            NavigationService.Configure(typeof(SettingsViewModel).FullName, typeof(SettingsPage));
        }

        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();
    }
}
