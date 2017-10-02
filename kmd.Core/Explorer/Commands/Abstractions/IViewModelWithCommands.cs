using kmd.Core.Explorer.Commands.Abstractions;

namespace kmd.Core.Commands.Abstractions
{
    public interface IViewModelWithCommands
    {
        CommandBindings CommandBindings { get; }
    }
}