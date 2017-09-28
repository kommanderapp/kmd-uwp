using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using kmd.Storage.Extensions;
using kmd.Core.Explorer.Contracts;
using kdm.Core.Services.Contracts;
using kmd.Core.Helpers;
using kdm.Core.Commands.Abstractions;
using kdm.Core.Explorer.Commands.Abstractions;

namespace kmd.Core.Explorer
{
    public class ExplorerViewModel : ViewModelBase, IExplorerModelState, IViewModelWithCommands
    {
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

        public bool IsHiglightVisible
        {
            get
            {
                return _isHiglightVisible;
            }
            internal set
            {
                Set(ref _isHiglightVisible, value);
            }
        }

        private bool _isHiglightVisible = false;

        public string HiglightQuery
        {
            get
            {
                return _higlightQuery;
            }
            set
            {
                Set(ref _higlightQuery, value);
            }
        }

        private string _higlightQuery = null;

        public ExplorerModelViewStates State { get; set; }

        public IExplorerModel Model { get; }

        private readonly IStorageFolderLister _folderLister;
        private readonly IStorageFolderFilter _folderFilter;
        private readonly IStorageFolderExpander _folderExpander;
        private readonly IFolderPickerService _folderPickerService;
        private readonly ICilpboardService _cilpboardService;
        private readonly IPathService _pathService;
        private readonly IFileLauncher _fileLauncher;
        private readonly IDialogService _dialogService;

        private DateTimeOffset _higlightQueryDate;

        private CancellationTokenSource _tokenSource = new CancellationTokenSource();

        public ICommand NavigateCommand { get; private set; }
        public ICommand NavigateByPathCommand { get; private set; }
        public ICommand ExpandViewCommand { get; private set; }
        public ICommand FilterViewCommand { get; private set; }
        public ICommand HiglightCommand { get; private set; }
        public ICommand OpenStorageItemCommand { get; private set; }
        public ICommand NavigateToParentCommand { get; private set; }
        public ICommand RequestCancelCommand { get; private set; }

        public ICommand ItemPathToClipboardCommand { get; private set; }
        public ICommand CopyCommand { get; private set; }
        public ICommand CutCommand { get; private set; }
        public ICommand PasteCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand RenameCommand { get; private set; }

        public CommandBindings CommandBindings { get; internal set; }

        public ExplorerViewModel(IFolderPickerService folderPickerService,
            IStorageFolderLister folderLister,
            IStorageFolderExpander folderExpander,
            IStorageFolderFilter folderFilter,
            IPathService pathService,
            IFileLauncher fileLauncher,
            ICilpboardService cilpboardService,
            IDialogService dialogService,
            ICommandBindingsProvider commandBindingsProvider)
        {
            _folderPickerService = folderPickerService ?? throw new ArgumentNullException(nameof(folderPickerService));
            _folderLister = folderLister ?? throw new ArgumentNullException(nameof(folderLister));
            _folderExpander = folderExpander ?? throw new ArgumentNullException(nameof(folderExpander));
            _folderFilter = folderFilter ?? throw new ArgumentNullException(nameof(folderFilter));
            _pathService = pathService ?? throw new ArgumentOutOfRangeException(nameof(pathService));
            _fileLauncher = fileLauncher ?? throw new ArgumentNullException(nameof(fileLauncher));
            _cilpboardService = cilpboardService ?? throw new ArgumentNullException(nameof(cilpboardService));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));

            NavigateCommand = new RelayCommand<IStorageFolder>(async (folder) => { await Model.GoToAsync(folder, _tokenSource.Token); });
            ExpandViewCommand = new RelayCommand(async () => { await Model.ExpandAsync(_tokenSource.Token); });
            FilterViewCommand = new RelayCommand<string>(async (text) => { await Model.FilterAsync(text, _tokenSource.Token); });

            NavigateByPathCommand = new RelayCommand<string>(async (path) => { await NavigateByPathAsync(path); });
            HiglightCommand = new RelayCommand<string>(Higlight);
            OpenStorageItemCommand = new RelayCommand(async () => { await OpenStorageItemAsync(); });
            NavigateToParentCommand = new RelayCommand(async () => { await NavigateToParrentAsync(); });
            RequestCancelCommand = new RelayCommand(RequestCancel);
            CopyCommand = new RelayCommand(CopyCurrentItem);
            CutCommand = new RelayCommand(CutCurrentItem);
            DeleteCommand = new RelayCommand(DeleteCurrentItemAsync);
            PasteCommand = new RelayCommand(PasteToCurrentFolder);
            ItemPathToClipboardCommand = new RelayCommand(ItemPathToClipboard);
            Model = new ExplorerModel((this as IExplorerModelState), folderLister, folderExpander, folderFilter);
            CommandBindings = commandBindingsProvider.GetBindings(Model);
        }

        protected void ItemPathToClipboard()
        {
            if (SelectedItem != null && SelectedItem.IsPhysical)
            {
                var data = new DataPackage();
                data.SetText(SelectedItem.Path);
                _cilpboardService.Set(data);
            }
        }

        protected async void PasteToCurrentFolder()
        {
            // TODO fix exception while copying folders
            IsBusy = true;

            var pastedItem = _cilpboardService.Get();
            var storageItems = await pastedItem.GetStorageItemsAsync();
            var changesMade = false;
            foreach (var item in storageItems)
            {
                if (item is IStorageFolder)
                {
                    await (item as IStorageFolder).CopyContentsRecursiveAsync(_currentFolder, _tokenSource.Token);
                    changesMade = true;
                }
                else if (item is IStorageFile)
                {
                    await (item as IStorageFile).CopyAsync(_currentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                await Model.GoToAsync(_currentFolder, _tokenSource.Token);
            }

            IsBusy = false;
        }

        protected void CopyCurrentItem()
        {
            if (SelectedItem != null && SelectedItem.IsPhysical)
            {
                var dataObject = new DataPackage
                {
                    RequestedOperation = DataPackageOperation.Copy
                };
                dataObject.SetStorageItems(new List<IStorageItem>() { SelectedItem.StorageItem });
                _cilpboardService.Set(dataObject);
            }
        }

        protected void CutCurrentItem()
        {
            if (SelectedItem != null && SelectedItem.IsPhysical)
            {
                var dataObject = new DataPackage
                {
                    RequestedOperation = DataPackageOperation.Move
                };
                dataObject.SetStorageItems(new List<IStorageItem>() { SelectedItem.StorageItem });
                _cilpboardService.Set(dataObject);
            }
        }

        protected async void DeleteCurrentItemAsync()
        {
            if (SelectedItem != null && SelectedItem.IsPhysical)
            {
                await _dialogService.ShowMessage("Explorer_DeleteFile_Message".GetLocalized(),
                    "Explorer_DeleteFile_Title".GetLocalized(),
                    "Explorer_DeleteFile_ConfirmButtonText".GetLocalized(),
                    "Explorer_DeleteFile_CancelButtonText".GetLocalized(),
                    async (accepted) =>
                    {
                        if (accepted)
                        {
                            await _selectedItem.StorageItem.DeleteAsync();
                            ExplorerItems.Remove(SelectedItem);
                        }
                    }
             );
            }
        }

        protected async Task NavigateByPathAsync(string path)
        {
            if (path == CurrentFolder.Path) { return; }
        }

        protected async Task NavigateToParrentAsync()
        {
            IsBusy = true;

            if (Model.IsViewInState(ExplorerModelViewStates.Default))
            {
                var parentFolder = await (_currentFolder as IStorageItem2)?.GetParentAsync();
                if (parentFolder != null)
                {
                    await Model.GoToAsync(parentFolder, _tokenSource.Token);
                }
            }
            else
            {
                // if it is expanded or filtered view, just reset view to normal state
                await Model.GoToAsync(_currentFolder, _tokenSource.Token);
            }

            IsBusy = false;
        }

        protected void RequestCancel()
        {
            _tokenSource.Cancel();
            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();
        }

        protected async Task OpenStorageItemAsync()
        {
            IsBusy = true;

            if (SelectedItem.IsFolder)
            {
                await Model.GoToAsync(SelectedItem.AsFolder, _tokenSource.Token);
            }
            else
            {
                await _fileLauncher.LaunchAsync(SelectedItem.AsFile);
            }

            IsBusy = false;
        }

        protected async void Higlight(string qry)
        {
            var now = DateTimeOffset.Now;

            if ((now - _higlightQueryDate).TotalSeconds > 0.8)
            {
                HiglightQuery = qry;
            }
            else
            {
                HiglightQuery += qry;
            }

            _higlightQueryDate = now;

            var elem = ExplorerItems.Where(x => x.Name.StartsWith(_higlightQuery, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefault();
            if (elem != null)
            {
                SelectedItem = elem;
            }
        }

        protected async Task<IEnumerable<IExplorerItem>> AppendAdditionalItems(IEnumerable<IExplorerItem> items)
        {
            var itemsList = new List<IExplorerItem>(items);
            var upperFolder = await ((StorageFolder)CurrentFolder).GetParentAsync();
            if (upperFolder != null)
            {
                var upperFolderModel = await UpperFolderLinkModel.CreateAsync(upperFolder);
                itemsList.Insert(0, upperFolderModel);
            }

            return itemsList;
        }
    }
}