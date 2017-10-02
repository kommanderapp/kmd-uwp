using GalaSoft.MvvmLight;
using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Helpers;
using kmd.Core.Services.Contracts;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Models;
using kmd.Storage.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using System.Windows.Input;
using kmd.Core.Explorer.Commands;

namespace kmd.Core.Explorer
{
    public class ExplorerViewModel : ViewModelBase, IExplorerViewModel
    {
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

            NavigateCommand = CommandBindings.OfType(typeof(NavigateCommand));
        }

        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
        public CommandBindings CommandBindings { get; internal set; }

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

        public FilterOptions FilterOptions { get; set; }

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

        public ExplorerItemsStates ItemsState { get; set; }
        public DateTimeOffset LastTypedCharacterDate { get; set; }
        public ICommand NavigateCommand { get; }

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

        public IExplorerItem SelectedItemBeforeExpanding { get; set; }

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

        public string TypedText { get; set; }

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

        protected readonly IExplorerItemMapper _explorerItemMapper;
        protected readonly IStorageFolderLister _folderLister;
        protected readonly IStorageFolderRootsExpander _folderRootsExpander;

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

        protected async Task OnExplorerItemsUpdateAsync()
        {
            TypedText = string.Empty;
            await AppendAdditionalItems();
            UpdateSelectedItem();
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

        private IStorageFolder _currentFolder = null;

        private ObservableCollection<IStorageFolder> _currentFolderExpandedRoots;

        private ObservableCollection<IExplorerItem> _explorerItems;

        private bool _isBusy = false;

        private bool _isPathBoxFocused = false;

        private IExplorerItem _selectedItem = null;

        private ObservableCollection<IExplorerItem> _selectedItems;
    }
}