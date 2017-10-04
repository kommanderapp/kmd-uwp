using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Core.Services.Impl;
using kmd.Core.Explorer;
using kmd.Storage.Contracts;
using kmd.Storage.Impl;
using Microsoft.Extensions.DependencyInjection;
using kmd.Core.Explorer.Commands;
using System;
using Microsoft.Practices.ServiceLocation;

namespace kmd.Core.DI
{
    public static class CoreRegistration
    {
        public static void AddCoreServices(this IServiceCollection sp)
        {
            sp.AddTransient<ExplorerViewModel>();
            sp.AddSingleton<IExplorerItemMapper, ExplorerItemMapper>();
            sp.AddSingleton<ICilpboardService, CilpboardService>();
            sp.AddSingleton<IPathService, PathService>();
            sp.AddSingleton<IFileLauncher, FileLauncher>();
            sp.AddSingleton<IFolderPickerService, FolderPickerService>();
            sp.AddSingleton<IStorageFolderRootsExpander, StorageFolderRootsExpander>();
            sp.AddSingleton<IStorageFolderExploder, StorageFolderExploder>();
            sp.AddSingleton<IStorageFolderLister, StorageFolderLister>();
            sp.AddSingleton<IStorageFolderFilter, StorageFolderFilter>();
            sp.AddExplorerDefaultCommands();
        }

        public static void AddExplorerDefaultCommands(this IServiceCollection sp)
        {
            sp.AddSingleton<IExplorerCommandBindingsProvider, ExplorerCommandBindingsProvider>();
            sp.AddSingleton<GoToPathBoxCommand>();
            sp.AddSingleton<CancelOperationsCommand>();
            sp.AddSingleton<CopySelectedItemCommand>();
            sp.AddSingleton<CutSelectedItemCommand>();
            sp.AddSingleton<DeleteSelectedItemCommand>();
            sp.AddSingleton<ExplodeCurrentFolderCommand>();
            sp.AddSingleton<FilterCommand>();
            sp.AddSingleton<ItemPathToClipboardCommand>();
            sp.AddSingleton<NavigateByPathCommand>();
            sp.AddSingleton<NavigateCommand>();
            sp.AddSingleton<NavigateToParrentCommand>();
            sp.AddSingleton<OpenSelectedItemCommand>();
            sp.AddSingleton<PasteToCurrentFolderCommand>();
            sp.AddSingleton<TypingHiglightCommand>();
            sp.AddSingleton<NavigateForwardCommand>();
            sp.AddSingleton<NavigateBackwardCommand>();
        }
    }
}