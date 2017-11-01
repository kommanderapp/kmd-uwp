namespace kmd.Core.Command
{
    public interface IViewModelWithCommandBindings
    {
        CommandBindings CommandBindings { get; }
    }
}