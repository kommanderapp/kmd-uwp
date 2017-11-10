using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Hotkeys;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand("NavigateForwardC", "Navigate forward", ModifierKeys.Control, VirtualKey.Right)]
    public class NavigateForwardCommand : ExplorerCommandBase
    {
        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.NavigationHistory.CanGoForward;
        }

        protected override void OnExecuteAsync(IExplorerViewModel vm)
        {
            var forwardFolder = vm.NavigationHistory.NavigateForward();
            if (forwardFolder != null)
                vm.CurrentFolder = forwardFolder;
        }
    }
}
