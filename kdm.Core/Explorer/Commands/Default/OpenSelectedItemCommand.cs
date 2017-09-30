using kdm.Core.Explorer.Commands.Abstractions;
using kmd.Storage.Contracts;
using Windows.System;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand(key: VirtualKey.Enter)]
    internal class OpenSelectedItemCommand : ExplorerCommand
    {
        private IFileLauncher _fileLauncher;

        public OpenSelectedItemCommand(IFileLauncher fileLauncher)
        {
            _fileLauncher = fileLauncher;
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            Model.ViewState.IsBusy = true;

            var selectedItem = Model.ViewState.SelectedItem;
            if (selectedItem.IsFolder)
            {
                await Model.GoToAsync(selectedItem.AsFolder);
            }
            else
            {
                await _fileLauncher.LaunchAsync(selectedItem.AsFile);
            }

            Model.ViewState.IsBusy = false;
        }
    }
}