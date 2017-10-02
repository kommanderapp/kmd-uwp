using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand]
    internal class NavigateByPathCommand : ExplorerCommandBase
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}