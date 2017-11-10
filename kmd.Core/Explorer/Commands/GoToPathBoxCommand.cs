using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Hotkeys;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand("GoToPathBox", "Go to PathBox", ModifierKeys.Control, VirtualKey.Q)]
    public class GoToPathBoxCommand : ExplorerCommandBase
    {
        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return true;
        }

        protected override void OnExecuteAsync(IExplorerViewModel vm)
        {
            vm.IsPathBoxFocused = true;
        }
    }
}
