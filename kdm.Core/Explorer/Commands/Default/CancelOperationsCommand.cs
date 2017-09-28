using kdm.Core.Explorer.Commands.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kmd.Core.Explorer.Contracts;
using System.Threading;
using Windows.System;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand(key: VirtualKey.Escape)]
    internal class CancelOperationsCommand : ExplorerCommand
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            Model.InternalState.CancellationTokenSource.Cancel();
            Model.InternalState.CancellationTokenSource.Dispose();
            Model.InternalState.CancellationTokenSource = new CancellationTokenSource();
            await Task.FromResult(0);
        }
    }
}