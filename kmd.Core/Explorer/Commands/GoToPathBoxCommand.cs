using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Hotkeys;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.Q)]
    public class GoToPathBoxCommand : ExplorerCommandBase
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            ViewModel.IsPathBoxFocused = true;
        }
    }
}