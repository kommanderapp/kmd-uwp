using kdm.Core.Explorer.Commands.Abstractions;
using kdm.Core.Explorer.Commands.Configuration;
using kdm.Core.Services.Contracts;
using kdm.Core.Services.Impl;
using kmd.Core.Explorer;
using kmd.Storage.Contracts;
using kmd.Storage.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace kdm.Core.DI
{
    public static class CoreRegistration
    {
        public static void AddCoreServices(this IServiceCollection sp)
        {
            sp.AddTransient<ExplorerViewModel>();
            sp.AddSingleton<IExplorerItemMapper, ExplorerItemMapper>();
            sp.AddSingleton<ICommandBindingsProvider, CommandBindingsProvider>();
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
    }
}