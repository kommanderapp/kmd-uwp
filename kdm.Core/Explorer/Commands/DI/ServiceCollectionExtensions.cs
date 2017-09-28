using kdm.Core.Explorer.Commands.Default;
using kdm.Core.Services.Contracts;
using kdm.Core.Services.Impl;
using kmd.Core.Explorer;
using kmd.Storage.Contracts;
using kmd.Storage.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace kdm.Core.Explorer.Commands.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void AddExplorerDefaultCommands(this IServiceCollection sp)
        {
            sp.AddTransient<CancelOperationsCommand>();
            sp.AddTransient<CopySelectedItemCommand>();
            sp.AddTransient<CutSelectedItemCommand>();
            sp.AddTransient<DeleteSelectedItemCommand>();
            sp.AddTransient<ExpandCommand>();
            sp.AddTransient<FilterCommand>();
            sp.AddTransient<ItemPathToClipboardCommand>();
            sp.AddTransient<NavigateByPathCommand>();
            sp.AddTransient<NavigateCommand>();
            sp.AddTransient<NavigateToParrentCommand>();
            sp.AddTransient<OpenSelectedItemCommand>();
            sp.AddTransient<PasteToCurrentFolderCommand>();
            sp.AddTransient<TypingHiglightCommand>();
        }
    }
}