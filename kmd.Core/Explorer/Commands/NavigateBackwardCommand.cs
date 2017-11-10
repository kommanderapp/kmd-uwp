using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Hotkeys;
using Windows.System;
using kmd.Core.Explorer.Contracts;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand("NavigateBackward", "Navigate backward", ModifierKeys.Control, VirtualKey.Left)]
    public class NavigateBackwardCommand : ExplorerCommandBase
    {
        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.NavigationHistory.CanGoBackward;
        }

        protected override void OnExecuteAsync(IExplorerViewModel vm)
        {
            var backFolder = vm.NavigationHistory.NavigateBackward();
            if (backFolder != null)
                vm.CurrentFolder = backFolder;
        }
    }
}
