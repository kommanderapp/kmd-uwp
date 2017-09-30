using kdm.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Contracts;
using System;
using Windows.Storage;
using Windows.System;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand(key: VirtualKey.Back)]
    public class NavigateToParrentCommand : ExplorerCommand
    {
        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            Model.ViewState.IsBusy = true;

            if (Model.InternalState.ItemsState == ExplorerItemsStates.Default)
            {
                var parentFolder = await (Model.ViewState.CurrentFolder as IStorageItem2)?.GetParentAsync();
                if (parentFolder != null)
                {
                    await Model.GoToAsync(parentFolder);
                }
            }
            else
            {
                // if it is expanded or filtered view, just reset view to normal state
                await Model.RefreshAsync();
            }

            Model.ViewState.IsBusy = false;
        }
    }
}