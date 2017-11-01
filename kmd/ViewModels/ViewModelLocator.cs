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

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public LocationsViewModel LocationsViewModel => ServiceLocator.Current.GetInstance<LocationsViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        private void ConfigureNavigationService()
        {
            NavigationService.Configure(typeof(MainViewModel).FullName, typeof(MainPage));
            NavigationService.Configure(typeof(LocationsViewModel).FullName, typeof(LocationsPage));
            NavigationService.Configure(typeof(SettingsViewModel).FullName, typeof(SettingsPage));
        }
    }
}
