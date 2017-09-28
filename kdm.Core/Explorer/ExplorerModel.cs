using kmd.Core.Explorer.Contracts;
using kmd.Storage.Contracts;
using Stateless;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Explorer
{
    public class ExplorerModel : IExplorerModel
    {
        public IExplorerViewState ViewState { get; }
        public IExplorerInternalState InternalState { get; }

        private readonly IStorageFolderLister _folderLister;
        private readonly IStorageFolderExpander _folderExpander;

        public ExplorerModel(IExplorerViewState viewState,
            IStorageFolderLister folderLister,
            IStorageFolderExpander folderExpander)
        {
            ViewState = viewState ?? throw new ArgumentNullException(nameof(viewState));
            InternalState = new ExplorerInternalState();

            _folderLister = folderLister ?? throw new ArgumentNullException(nameof(folderLister));
            _folderExpander = folderExpander ?? throw new ArgumentNullException(nameof(folderExpander));
        }

        public async Task GoToAsync(IStorageFolder folder)
        {
            if (folder == null) return;

            ViewState.IsBusy = true;

            var expandedRoots = await _folderExpander.ExpandOuterAsync(folder, InternalState.CancellationTokenSource.Token);
            ViewState.CurrentFolderExpandedRoots = new ObservableCollection<IStorageFolder>(expandedRoots);

            var items = await _folderLister.ListAsync(folder, InternalState.CancellationTokenSource.Token);

            ViewState.CurrentFolder = folder;
            InternalState.ItemsState = ExplorerItemsStates.Default;
            ViewState.ExplorerItems = new ObservableCollection<IExplorerItem>(items);

            ViewState.IsBusy = false;
        }

        public async Task RefreshAsync()
        {
            await GoToAsync(ViewState.CurrentFolder);
        }
    }

    public class ExplorerInternalState : IExplorerInternalState
    {
        public IExplorerItem SelectedItemBeforeExpanding { get; set; }
        public string TypedText { get; set; }
        public FilterOptions FilterOptions { get; set; }
        public ExplorerItemsStates ItemsState { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
        public DateTimeOffset LastTypedCharacterDate { get; set; }
    }
}