using kdm.Core.Explorer.Commands.Abstractions;
using kdm.Core.Explorer.Commands.Configuration;
using kdm.Core.Services.Contracts;
using kmd.Storage.Contracts;
using System;

namespace kdm.Core.Explorer.Commands
{
    [ExplorerCommand]
    public class FilterCommand : ExplorerCommandBase
    {
        protected readonly IStorageFolderFilter _storageFolderFilter;
        protected readonly IExplorerItemMapper _explorerItemMapper;

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
    }
}