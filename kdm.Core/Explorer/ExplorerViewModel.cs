using GalaSoft.MvvmLight;
using kdm.Core.Explorer.Commands.Abstractions;
using kdm.Core.Helpers;
using kdm.Core.Services.Contracts;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Models;
using kmd.Storage.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Explorer
{
    public class ExplorerViewModel : ViewModelBase, IExplorerViewModel
    {
        #region View State

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                Set(ref _isBusy, value);
            }
        }

        private bool _isBusy = false;

        public IStorageFolder CurrentFolder
        {
            get
            {
                return _currentFolder;
            }
            set
            {
                Set(ref _currentFolder, value);
            }
        }

        private IStorageFolder _currentFolder = null;

        public ObservableCollection<IStorageFolder> CurrentFolderExpandedRoots
        {
            get
            {
                return _currentFolderExpandedRoots;
            }
            set
            {
                Set(ref _currentFolderExpandedRoots, value);
            }
        }

        private ObservableCollection<IStorageFolder> _currentFolderExpandedRoots;

        public bool IsPathBoxFocused
        {
            get
            {
                return _isPathBoxFocused;
            }
            set
            {
                Set(ref _isPathBoxFocused, value);
            }
        }

        private bool _isPathBoxFocused = false;

        public IExplorerItem SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                Set(ref _selectedItem, value);
            }
        }

        private IExplorerItem _selectedItem = null;

        public ObservableCollection<IExplorerItem> ExplorerItems
        {
            get
            {
                return _explorerItems;
            }
            set
            {
                Set(ref _explorerItems, value);
                OnExplorerItemsUpdateAsync().FireAndForget();
            }
        }

        private ObservableCollection<IExplorerItem> _explorerItems;

        public ObservableCollection<IExplorerItem> SelectedItems
        {
            get
            {
                return _selectedItems;
            }
            set
            {
                Set(ref _selectedItems, value);
            }
        }

        private ObservableCollection<IExplorerItem> _selectedItems;

        #endregion View State

        #region Internal State

        public IExplorerItem SelectedItemBeforeExpanding { get; set; }
        public string TypedText { get; set; }
        public FilterOptions FilterOptions { get; set; }
        public ExplorerItemsStates ItemsState { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
        public DateTimeOffset LastTypedCharacterDate { get; set; }

        #endregion Internal State

        #region Services

        protected readonly IStorageFolderRootsExpander _folderRootsExpander;
        protected readonly IStorageFolderLister _folderLister;
        protected readonly IExplorerItemMapper _explorerItemMapper;

        #endregion Services

        public CommandBindings CommandBindings { get; internal set; }

        public ExplorerViewModel(ICommandBindingsProvider commandBindingsProvider,
            IStorageFolderRootsExpander folderRootsExpander,
            IStorageFolderLister folderLister,
            IExplorerItemMapper explorerItemMapper)
        {
            _folderRootsExpander = folderRootsExpander ?? throw new ArgumentNullException(nameof(folderRootsExpander));
            _folderLister = folderLister ?? throw new ArgumentNullException(nameof(folderLister));
            _explorerItemMapper = explorerItemMapper ?? throw new ArgumentNullException(nameof(explorerItemMapper));

            if (commandBindingsProvider == null) throw new ArgumentNullException(nameof(commandBindingsProvider));
            CommandBindings = commandBindingsProvider.GetBindings(this);
        }

        protected async Task OnExplorerItemsUpdateAsync()
        {
            TypedText = string.Empty;
            await AppendAdditionalItems();
            UpdateSelectedItem();
        }

        protected async Task AppendAdditionalItems()
        {
            if (ItemsState == ExplorerItemsStates.Default && CurrentFolder != null)
            {
                var upperFolder = await ((StorageFolder)CurrentFolder).GetParentAsync();
                if (upperFolder != null)
                {
                    var upperFolderModel = await ExplorerUpperFolderLinkItem.CreateAsync(upperFolder);
                    ExplorerItems.Insert(0, upperFolderModel);
                }
            }
        }

        protected void UpdateSelectedItem()
        {
            IExplorerItem selectedItem = null;
            if (SelectedItemBeforeExpanding != null)
            {
                selectedItem = ExplorerItems.FirstOrDefault(x => x.Path == SelectedItemBeforeExpanding.Path);
            }

            if (selectedItem == null)
            {
                selectedItem = ExplorerItems.FirstOrDefault();
            }

            SelectedItem = selectedItem;
            SelectedItemBeforeExpanding = null;
        }

        public async Task GoToAsync(IStorageFolder folder)

        {
            if (folder == null) return;

            IsBusy = true;

            var expandedRoots = await _folderRootsExpander.ExpandOuterAsync(folder, CancellationTokenSource.Token);

            CurrentFolderExpandedRoots = new ObservableCollection<IStorageFolder>(expandedRoots);

            var items = await _folderLister.ListAsync(folder, CancellationTokenSource.Token);

            CurrentFolder = folder;
            ItemsState = ExplorerItemsStates.Default;
            ExplorerItems = await _explorerItemMapper.MapAsync(items);
            IsBusy = false;
        }

        public async Task RefreshAsync()

        {
            await GoToAsync(CurrentFolder);
        }
    }
}