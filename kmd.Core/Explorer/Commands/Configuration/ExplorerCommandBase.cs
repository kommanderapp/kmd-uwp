using kmd.Core.Command;
using kmd.Core.Explorer.Contracts;

namespace kmd.Core.Explorer.Commands.Configuration
{
    public abstract class ExplorerCommandBase : CommandBase
    {
        protected override bool OnCanExecute(object parameter)
        {
            if (parameter is IExplorerViewModel)
            {
                return OnCanExecute((IExplorerViewModel)parameter);
            }
            return false;
        }

        protected abstract bool OnCanExecute(IExplorerViewModel vm);

        protected override void OnExecute(object parameter)
        {
            var vm = (IExplorerViewModel)parameter;
            OnExecuteAsync(vm);
        }

        protected abstract void OnExecuteAsync(IExplorerViewModel vm);
    }
}