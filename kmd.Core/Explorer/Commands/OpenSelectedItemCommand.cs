using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Hotkeys;
using kmd.Storage.Contracts;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand("OpenSelectedItem", "Open selected item", ModifierKeys.None, VirtualKey.Enter)]
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
            return vm.SelectedItem != null;
        }

        protected override async void OnExecuteAsync(IExplorerViewModel vm)
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
