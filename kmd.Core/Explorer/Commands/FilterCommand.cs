using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Storage.Contracts;
using System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand]
    public class FilterCommand : ExplorerCommandBase
    {
        public FilterCommand(IStorageFolderFilter storageFolderFilter, IExplorerItemMapper explorerItemMapper)
        {
            _storageFolderFilter = storageFolderFilter ?? throw new ArgumentNullException(nameof(storageFolderFilter));
            _explorerItemMapper = explorerItemMapper ?? throw new ArgumentNullException(nameof(explorerItemMapper));
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            ViewModel.IsBusy = true;

            var filteredItems = await _storageFolderFilter.FilterAsync(ViewModel.CurrentFolder,
                ViewModel.FilterOptions, ViewModel.CancellationTokenSource.Token);

            ViewModel.ExplorerItems = await _explorerItemMapper.MapAsync(filteredItems);

            ViewModel.IsBusy = false;
        }

        protected readonly IExplorerItemMapper _explorerItemMapper;
        protected readonly IStorageFolderFilter _storageFolderFilter;
    }
}