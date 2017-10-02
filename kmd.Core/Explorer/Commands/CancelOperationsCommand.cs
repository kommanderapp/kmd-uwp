using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using System.Threading;
using System.Threading.Tasks;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.Escape)]
    internal class CancelOperationsCommand : ExplorerCommandBase
    {
        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }

        protected override async void OnExecute(object parameter)
        {
            ViewModel.CancellationTokenSource.Cancel();
            ViewModel.CancellationTokenSource.Dispose();
            ViewModel.CancellationTokenSource = new CancellationTokenSource();
            await Task.FromResult(0);
        }
    }
}