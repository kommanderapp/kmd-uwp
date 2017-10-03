using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Core.Services.Impl;
using kmd.Core.Explorer;
using kmd.Storage.Contracts;
using kmd.Storage.Impl;
using Microsoft.Extensions.DependencyInjection;
using Autofac;
using kmd.Core.Explorer.Commands;

namespace kmd.Core.DI
{
    public static class CoreRegistration
    {
        public static void RegisterCoreServices(this ContainerBuilder builder)
        {
            builder.RegisterType<ExplorerViewModel>().AsSelf().OnActivating(ctx =>
            {
            });

            builder.RegisterType<ExplorerItemMapper>().As<IExplorerItemMapper>().SingleInstance();
            builder.RegisterType<CilpboardService>().As<ICilpboardService>().SingleInstance();
            builder.RegisterType<PathService>().As<IPathService>().SingleInstance();
            builder.RegisterType<FileLauncher>().As<IFileLauncher>().SingleInstance();
            builder.RegisterType<FolderPickerService>().As<IFolderPickerService>().SingleInstance();
            builder.RegisterType<StorageFolderRootsExpander>().As<IStorageFolderRootsExpander>().SingleInstance();
            builder.RegisterType<StorageFolderExploder>().As<IStorageFolderExploder>().SingleInstance();
            builder.RegisterType<StorageFolderLister>().As<IStorageFolderLister>().SingleInstance();
            builder.RegisterType<StorageFolderFilter>().As<IStorageFolderFilter>().SingleInstance();

            builder.RegisterExplorerCommands();
        }

        public static void RegisterExplorerCommands(this ContainerBuilder builder)
        {
            builder.RegisterType<GoToPathBoxCommand>();
            builder.RegisterType<CancelOperationsCommand>();
            builder.RegisterType<CopySelectedItemCommand>();
            builder.RegisterType<CutSelectedItemCommand>();
            builder.RegisterType<DeleteSelectedItemCommand>();
            builder.RegisterType<ExplodeCurrentFolderCommand>();
            builder.RegisterType<FilterCommand>();
            builder.RegisterType<ItemPathToClipboardCommand>();
            builder.RegisterType<NavigateByPathCommand>();
            builder.RegisterType<NavigateCommand>();
            builder.RegisterType<NavigateToParrentCommand>();
            builder.RegisterType<OpenSelectedItemCommand>();
            builder.RegisterType<PasteToCurrentFolderCommand>();
            builder.RegisterType<TypingHiglightCommand>();
        }
    }
}