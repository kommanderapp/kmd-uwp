using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Hotkeys;
using kmd.Storage.Contracts;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand("ExplodeCurrentFolder", "Explode current folder", ModifierKeys.Control, VirtualKey.B)]
    public class ExplodeCurrentFolderCommand : ExplorerCommandBase
    {
        public ExplodeCurrentFolderCommand(IStorageFolderExploder storageFolderExploder,
            IExplorerItemMapper explorerItemMapper)
        {
            _storageFolderExploder = storageFolderExploder ?? throw new ArgumentNullException(nameof(storageFolderExploder));
            _explorerItemMapper = explorerItemMapper ?? throw new ArgumentNullException(nameof(explorerItemMapper));
        }

        protected readonly IExplorerItemMapper _explorerItemMapper;

        protected readonly IStorageFolderExploder _storageFolderExploder;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.ItemsState != ExplorerItemsStates.Expanded;
        }

        protected override async void OnExecuteAsync(IExplorerViewModel vm)
        {
            vm.IsBusy = true;

            var items = await _storageFolderExploder.ExplodeAsync(vm.CurrentFolder, vm.CancellationTokenSource.Token);

            vm.ItemsState = ExplorerItemsStates.Expanded;
            vm.SelectedItemBeforeExpanding = vm.SelectedItem;

            vm.ExplorerItems = await _explorerItemMapper.MapAsync(items);

            vm.IsBusy = false;
        }
    }
}
