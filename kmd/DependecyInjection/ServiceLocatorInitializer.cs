using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.CommonServiceLocator;
using GalaSoft.MvvmLight.Views;
using kmd.Core.DI;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
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
            builder.RegisterType<PromptService>().As<IPromptService>();
            builder.RegisterType<LocationAccessService>().AsSelf().SingleInstance();

            // ViewModels
            builder.RegisterType<ShellViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<MainViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<SettingsViewModel>().AsSelf().SingleInstance();

            sc.AddCoreServices();
            builder.Populate(sc);

            var container = builder.Build();
            var serviceLocator = new AutofacServiceLocator(container);

            ExplorerCommandBindingsProvider.Resolve = (t) => serviceLocator.GetService(t);
            //CoreRegistration.RegisterFactoryResolvers(serviceLocator);

            ServiceLocator.SetLocatorProvider(() => serviceLocator);
        }
    }
}
