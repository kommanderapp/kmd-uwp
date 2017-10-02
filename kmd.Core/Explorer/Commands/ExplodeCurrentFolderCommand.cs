using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Hotkeys;
using kmd.Storage.Contracts;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.B)]
    public class ExplodeCurrentFolderCommand : ExplorerCommandBase
    {
        public ExplodeCurrentFolderCommand(IStorageFolderExploder storageFolderExploder, IExplorerItemMapper explorerItemMapper)
        {
            _storageFolderExploder = storageFolderExploder ?? throw new ArgumentNullException(nameof(storageFolderExploder));
            _explorerItemMapper = explorerItemMapper ?? throw new ArgumentNullException(nameof(explorerItemMapper));
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            ViewModel.IsBusy = true;

            var items = await _storageFolderExploder.ExplodeAsync(ViewModel.CurrentFolder, ViewModel.CancellationTokenSource.Token);

            ViewModel.ItemsState = ExplorerItemsStates.Expanded;
            ViewModel.SelectedItemBeforeExpanding = ViewModel.SelectedItem;

            ViewModel.ExplorerItems = await _explorerItemMapper.MapAsync(items);

            ViewModel.IsBusy = false;
        }

        protected readonly IExplorerItemMapper _explorerItemMapper;
        protected readonly IStorageFolderExploder _storageFolderExploder;
    }
}