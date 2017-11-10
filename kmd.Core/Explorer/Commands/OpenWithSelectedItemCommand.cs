using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Hotkeys;
using kmd.Storage.Contracts;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand("OpenWithSelectedItemCommand", "Open with", ModifierKeys.Control, VirtualKey.O)]
    public class OpenWithSelectedItemCommand : ExplorerCommandBase
    {
        public OpenWithSelectedItemCommand(IFileLauncher fileLauncher)
        {
            _fileLauncher = fileLauncher ?? throw new ArgumentNullException(nameof(fileLauncher));
        }

        protected readonly IFileLauncher _fileLauncher;

        protected readonly NavigateCommand _navigateCommand;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.SelectedItem != null && vm.SelectedItem.IsFile;
        }

        protected override async void OnExecuteAsync(IExplorerViewModel vm)
        {
            var selectedItem = vm.SelectedItem;
            if (selectedItem.IsFile)
            {
                await _fileLauncher.LaunchAsync(selectedItem.AsFile, displayApplicationPicker: true);
            }
        }
    }
}
