using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Storage.Contracts;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.Enter)]
    internal class OpenSelectedItemCommand : ExplorerCommandBase
    {
        public OpenSelectedItemCommand(IFileLauncher fileLauncher)
        {
            _fileLauncher = fileLauncher;
        }

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }

        protected override async void OnExecute(object parameter)
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