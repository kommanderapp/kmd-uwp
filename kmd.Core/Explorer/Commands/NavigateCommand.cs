using kmd.Core.Command;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Services.Contracts;
using kmd.Storage.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand]
    public class NavigateCommand : ExplorerCommandBase
    {
        public NavigateCommand(IStorageFolderExploder folderExpander,
            IStorageFolderLister folderLister,
            IStorageFolderRootsExpander folderRootsExpander,
            IExplorerItemMapper explorerItemMapper)
        {
            _folderExpander = folderExpander ?? throw new ArgumentNullException(nameof(folderExpander));
            _folderLister = folderLister ?? throw new ArgumentNullException(nameof(folderLister));
            _folderRootsExpander = folderRootsExpander ?? throw new ArgumentNullException(nameof(folderRootsExpander));
            _explorerItemMapper = explorerItemMapper ?? throw new ArgumentNullException(nameof(explorerItemMapper));
        }

        protected readonly IExplorerItemMapper _explorerItemMapper;
        protected readonly IStorageFolderExploder _folderExpander;
        protected readonly IStorageFolderLister _folderLister;
        protected readonly IStorageFolderRootsExpander _folderRootsExpander;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return true;
        }

        protected override async void OnExecute(IExplorerViewModel vm)
        {
            vm.IsBusy = true;

            var expandedRoots = await _folderRootsExpander.ExpandOuterAsync(vm.CurrentFolder, vm.CancellationTokenSource.Token);

            vm.CurrentFolderExpandedRoots = new ObservableCollection<IStorageFolder>(expandedRoots);

            var items = await _folderLister.ListAsync(vm.CurrentFolder, vm.CancellationTokenSource.Token);

            vm.ItemsState = ExplorerItemsStates.Default;
            vm.ExplorerItems = await _explorerItemMapper.MapAsync(items);

            vm.IsBusy = false;
        }
    }
}