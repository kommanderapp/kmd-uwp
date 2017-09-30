using kdm.Core.Explorer.Commands.Abstractions;
using System;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand]
    internal class NavigateByPathCommand : ExplorerCommand
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