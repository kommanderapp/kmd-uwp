using kmd.Core.Command;
using kmd.Core.Explorer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            OnExecute(vm);
        }

        protected abstract void OnExecute(IExplorerViewModel vm);
    }
}