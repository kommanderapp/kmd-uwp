using kdm.Core.Explorer.Commands.Abstractions;

namespace kdm.Core.Commands.Abstractions
{
    public interface IViewModelWithCommands
    {
        CommandBindings CommandBindings { get; }
    }
}