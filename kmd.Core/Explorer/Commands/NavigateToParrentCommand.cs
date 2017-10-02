using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using System;
using Windows.Storage;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.Back)]
    public class NavigateToParrentCommand : ExplorerCommandBase
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            ViewModel.IsBusy = true;

            if (ViewModel.ItemsState == ExplorerItemsStates.Default)
            {
                var parentFolder = await (ViewModel.CurrentFolder as IStorageItem2)?.GetParentAsync();
                if (parentFolder != null)
                {
                    await ViewModel.GoToAsync(parentFolder);
                }
            }
            else
            {
                // if it is expanded or filtered view, just reset view to normal state
                await ViewModel.RefreshAsync();
            }

            ViewModel.IsBusy = false;
        }
    }
}