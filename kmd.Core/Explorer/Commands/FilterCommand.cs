using kmd.Core.Command;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
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

        protected readonly IExplorerItemMapper _explorerItemMapper;

        protected readonly IStorageFolderFilter _storageFolderFilter;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return true;
        }

        protected async override void OnExecuteAsync(IExplorerViewModel vm)
        {
            vm.IsBusy = true;

            var filteredItems = await _storageFolderFilter.FilterAsync(vm.CurrentFolder,
                vm.FilterOptions, vm.CancellationTokenSource.Token);

            vm.ExplorerItems = await _explorerItemMapper.MapAsync(filteredItems);

            vm.IsBusy = false;
        }
    }
}