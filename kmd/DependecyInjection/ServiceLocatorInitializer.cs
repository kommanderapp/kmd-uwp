using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using kdm.Core.DI;
using kdm.Core.Services.Impl;
using kmd.Services;
using kmd.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Practices.ServiceLocation;

namespace kmd.DependecyInjection
{
    public static class ServiceLocatorInitializer
    {
        public static void Initialize()
        {
            var builder = new ContainerBuilder();
            var sc = new ServiceCollection();

            sc.AddSingleton<NavigationServiceEx>();

            sc.AddSingleton<IDialogService, DialogService>();
            sc.AddSingleton<LocationAccessService>();

            // ViewModels
            sc.AddSingleton<ShellViewModel>();
            sc.AddSingleton<MainViewModel>();
            sc.AddSingleton<SettingsViewModel>();

            sc.AddCoreServices();

            builder.Populate(sc);
            var container = builder.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }
    }
}
