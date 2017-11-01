using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using System;
using Windows.Storage;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand("NavigateToParrent", "NavigateToParrent", key: VirtualKey.Back)]
    public class NavigateToParrentCommand : ExplorerCommandBase
    {
        public NavigateToParrentCommand(NavigateCommand navigateCommand)
        {
            _navigateCommand = navigateCommand ?? throw new ArgumentNullException(nameof(navigateCommand));
        }

        protected readonly NavigateCommand _navigateCommand;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return true;
        }

        protected async override void OnExecuteAsync(IExplorerViewModel vm)
        {
            if (vm.ItemsState == ExplorerItemsStates.Default)
            {
                var parentFolder = await (vm.CurrentFolder as IStorageItem2)?.GetParentAsync();
                if (parentFolder != null)
                {
                    vm.CurrentFolder = parentFolder;
                }
            }
            else
            {
                // if it is expanded or filtered view, just reset view to normal state
                vm.CurrentFolder = vm.CurrentFolder;
            }
        }
    }
}