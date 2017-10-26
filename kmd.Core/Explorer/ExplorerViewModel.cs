using GalaSoft.MvvmLight;
using kmd.Core.Command;
using kmd.Core.Explorer.Commands;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Models;
using kmd.Core.Helpers;
using kmd.Storage.Contracts;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Explorer
{
    public class ExplorerViewModel : ViewModelBase, IExplorerViewModel, IViewModelWithCommandBindings
    {
        public ExplorerViewModel(IExplorerCommandBindingsProvider commandBindingsProvider)
        {
            _commandBindingsProvider = commandBindingsProvider ?? throw new ArgumentNullException(nameof(commandBindingsProvider));
            CommandBindings = _commandBindingsProvider.GetBindings(this);
            NavigationHistory = new ExplorerNavigationHistory();
            SelectedItems = new ObservableCollection<IExplorerItem>();
        }

        public CancellationTokenSource CancellationTokenSource { get; set; } = new CancellationTokenSource();
        public CommandBindings CommandBindings { get; }

        public IStorageFolder CurrentFolder
        {
            get
            {
                return _currentFolder;
            }
            set
            {
                Set(ref _currentFolder, value);
                OnCurrentFolderUpdate();
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

        public bool CanGroup => SelectedItems.Count > 1 && SelectedItems.All(i => i.IsPhysical);

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

        public string LastTypedChar
        {
            get
            {
                return _lastTypedChar;
            }
            set
            {
                _lastTypedChar = value;
                OnCharTyped();
            }
        }

        public DateTimeOffset LastTypedCharacterDate { get; set; }

        public ExplorerNavigationHistory NavigationHistory { get; set; }

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
        protected readonly IExplorerCommandBindingsProvider _commandBindingsProvider;

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

        protected void OnCharTyped()
        {
            this.ExecuteCommand(typeof(TypingHiglightCommand));
        }

        protected void OnCurrentFolderUpdate()
        {
            this.ExecuteCommand(typeof(NavigateCommand));
            NavigationHistory.SetCurrent(CurrentFolder);
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

        private string _lastTypedChar;

        private IExplorerItem _selectedItem = null;

        private ObservableCollection<IExplorerItem> _selectedItems;
    }
}