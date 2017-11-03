using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Core.Services.Impl;
using kmd.Core.Explorer;
using kmd.Storage.Contracts;
using kmd.Storage.Impl;
using Microsoft.Extensions.DependencyInjection;
using kmd.Core.Command.Configuration;

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
            sp.AddSingleton<ILocationService, LocationService>();
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

            var commandDescriptors = CommandDescriptorProvider.GetCommandDescriptors();
            foreach (var commandDescriptor in commandDescriptors)
            {
                if (commandDescriptor is ExplorerCommandDescriptor explorerCommandDescriptor)
                {
                    sp.AddSingleton(explorerCommandDescriptor.CommandType);
                }
            }
        }
    }
}
