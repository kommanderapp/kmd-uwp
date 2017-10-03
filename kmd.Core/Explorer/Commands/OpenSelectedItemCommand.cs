using kmd.Core.Command;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Storage.Contracts;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.Enter)]
    public class OpenSelectedItemCommand : ExplorerCommandBase
    {
        public OpenSelectedItemCommand(IFileLauncher fileLauncher)
        {
            _fileLauncher = fileLauncher ?? throw new ArgumentNullException(nameof(fileLauncher));
        }

        protected readonly IFileLauncher _fileLauncher;

        protected readonly NavigateCommand _navigateCommand;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return true;
        }

        protected override async void OnExecute(IExplorerViewModel vm)
        {
            var selectedItem = vm.SelectedItem;
            if (selectedItem.IsFolder)
            {
                vm.CurrentFolder = selectedItem.AsFolder;
            }
            else
            {
                await _fileLauncher.LaunchAsync(selectedItem.AsFile);
            }
        }
    }
}