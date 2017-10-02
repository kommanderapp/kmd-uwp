using kmd.Core.Explorer.Contracts;

namespace kmd.Core.Explorer.Commands.Abstractions
{
    public interface ICommandBindingsProvider
    {
        CommandBindings GetBindings(IExplorerViewModel explorerViewModel);
    }
}