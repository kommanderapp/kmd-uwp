using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand]
    internal class NavigateByPathCommand : ExplorerCommandBase
    {
        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }

        protected override void OnExecute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}