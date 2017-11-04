using CommonServiceLocator;
using kmd.Core.Helpers;
using kmd.Services;
using kmd.ViewModels;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace kmd.Views
{
    public sealed partial class ShellPage : Page
    {
        private object _lastSelectedItem;
        public NavigationServiceEx NavigationService
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NavigationServiceEx>();
            }
        }

        public ShellPage()
        {
            InitializeComponent();

            NavigationService.Frame = shellFrame;
            NavigationService.Navigated += Frame_Navigated;
            PopulateNavItems();            
        }
        
        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e != null)
            {
                var vm = NavigationService.GetNameOfRegisteredPage(e.SourcePageType);
                var navigationItem = ShellNavigation.MenuItems?.FirstOrDefault(i => i is NavigationViewItem navItem && navItem.Tag.ToString() == vm);

                if (navigationItem != null)
                {
                    (navigationItem as NavigationViewItem).IsSelected = true;
                    _lastSelectedItem = navigationItem;
                }
            }
        }

        private void ShellNavigation_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigateToSettings();
            }
            else
            {
                Navigate(args.InvokedItem);
            }
        }

        private void NavigateToSettings()
        {
            NavigationService.Navigate(typeof(SettingsViewModel).FullName);
        }

        private void Navigate(object item)
        {
            if (item is string itemName)
            {
                var navigationItem = ShellNavigation.MenuItems.Cast<NavigationViewItem>().FirstOrDefault(x => x.Content == itemName);
                if (navigationItem != null)
                {
                    NavigationService.Navigate(navigationItem.Tag.ToString());
                }
            }
        }

        private void PopulateNavItems()
        {            
            ShellNavigation.MenuItems.Add(new NavigationViewItem() { Content = "Shell_Main".GetLocalized(), Tag = typeof(MainViewModel).FullName, Icon = new SymbolIcon(Symbol.Document) });
            ShellNavigation.MenuItems.Add(new NavigationViewItem() { Content = "Shell_Locations".GetLocalized(), Tag = typeof(LocationsViewModel).FullName, Icon = new SymbolIcon(Symbol.MapDrive) });            
        }
    }
}
