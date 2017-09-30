using kdm.Core.Explorer.Commands.Abstractions;
using kdm.Core.Explorer.Commands.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;

namespace kdm.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.Escape)]
    internal class CancelOperationsCommand : ExplorerCommandBase
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            ViewModel.CancellationTokenSource.Cancel();
            ViewModel.CancellationTokenSource.Dispose();
            ViewModel.CancellationTokenSource = new CancellationTokenSource();
            await Task.FromResult(0);
        }
    }
}