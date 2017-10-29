using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using System.Threading;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.Escape)]
    public class CancelOperationsCommand : ExplorerCommandBase
    {
        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return true;
        }

        protected override void OnExecuteAsync(IExplorerViewModel vm)
        {
            vm.CancellationTokenSource.Cancel();
            vm.CancellationTokenSource.Dispose();
            vm.CancellationTokenSource = new CancellationTokenSource();
        }
    }
}