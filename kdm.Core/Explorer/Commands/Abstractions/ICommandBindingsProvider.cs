using kmd.Core.Explorer.Contracts;

namespace kdm.Core.Explorer.Commands.Abstractions
{
    public interface ICommandBindingsProvider
    {
        CommandBindings GetBindings(IExplorerModel explorerModel);
    }
}