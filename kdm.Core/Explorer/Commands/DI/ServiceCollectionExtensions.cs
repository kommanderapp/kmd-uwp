using kdm.Core.Explorer.Commands.Default;
using Microsoft.Extensions.DependencyInjection;

namespace kdm.Core.Explorer.Commands.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void AddExplorerDefaultCommands(this IServiceCollection sp)
        {
            sp.AddTransient<GoToPathBoxCommand>();
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