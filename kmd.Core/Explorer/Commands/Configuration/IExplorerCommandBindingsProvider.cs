using kmd.Core.Command;
using kmd.Core.Explorer.Contracts;

namespace kmd.Core.Explorer.Commands.Configuration
{
    public interface IExplorerCommandBindingsProvider
    {
        CommandBindings GetBindings(IExplorerViewModel explorerViewModel);
    }
}