using kdm.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Hotkeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.Q)]
    public class GoToPathBoxCommand : ExplorerCommand
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            Model.ViewState.IsPathBoxFocused = true;
        }
    }
}