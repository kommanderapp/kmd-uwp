using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand]
    public class NavigateByPathCommand : ExplorerCommandBase
    {
        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return true;
        }

        protected override void OnExecuteAsync(IExplorerViewModel vm)
        {
            throw new NotImplementedException();
        }
    }
}