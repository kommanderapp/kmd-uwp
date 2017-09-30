using kdm.Core.Explorer.Commands.Abstractions;
using kdm.Core.Explorer.Commands.Configuration;
using kmd.Storage.Contracts;
using Windows.System;

namespace kdm.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.Enter)]
    internal class OpenSelectedItemCommand : ExplorerCommandBase
    {
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
            ViewModel.IsBusy = true;

            var selectedItem = ViewModel.SelectedItem;
            if (selectedItem.IsFolder)
            {
                await ViewModel.GoToAsync(selectedItem.AsFolder);
            }
            else
            {
                await _fileLauncher.LaunchAsync(selectedItem.AsFile);
            }

            ViewModel.IsBusy = false;
        }

        private IFileLauncher _fileLauncher;
    }
}