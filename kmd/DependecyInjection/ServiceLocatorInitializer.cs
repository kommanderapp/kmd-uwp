using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using kmd.Core.DI;
using kmd.Core.Services.Impl;
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

            builder.RegisterType<NavigationServiceEx>().AsSelf().SingleInstance();
            builder.RegisterType<DialogService>().As<IDialogService>();
            builder.RegisterType<LocationAccessService>().AsSelf().SingleInstance();

            // ViewModels
            builder.RegisterType<ShellViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<SettingsViewModel>().AsSelf().SingleInstance();

            builder.RegisterCoreServices();
            var container = builder.Build();

            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
        }
    }
}
