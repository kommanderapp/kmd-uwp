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
using kdm.Core.Explorer.Commands.Default;

namespace kmd.Core.Explorer
{
    public class ExplorerViewModel : ViewModelBase, IExplorerViewState, IViewModelWithCommands
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
                OnExplorerItemsUpdate();
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

        public IExplorerModel Model { get; }

        public CommandBindings CommandBindings { get; internal set; }

        public ExplorerViewModel(ICommandBindingsProvider commandBindingsProvider,
            IStorageFolderLister folderLister,
            IStorageFolderExpander folderExpander)
        {
            Model = new ExplorerModel((this as IExplorerViewState), folderLister, folderExpander);
            CommandBindings = commandBindingsProvider.GetBindings(Model);
        }

        protected void OnExplorerItemsUpdate()
        {
            Model.InternalState.TypedText = string.Empty;
            AppendAdditionalItems();
            UpdateSelectedItem();
        }

        protected async void AppendAdditionalItems()
        {
            if (Model.InternalState.ItemsState == ExplorerItemsStates.Default && CurrentFolder != null)
            {
                var upperFolder = await ((StorageFolder)CurrentFolder).GetParentAsync();
                if (upperFolder != null)
                {
                    var upperFolderModel = await UpperFolderLinkModel.CreateAsync(upperFolder);
                    ExplorerItems.Insert(0, upperFolderModel);
                }
            }
        }

        protected void UpdateSelectedItem()
        {
            IExplorerItem selectedItem = null;
            if (Model.InternalState.SelectedItemBeforeExpanding != null)
            {
                selectedItem = ExplorerItems.FirstOrDefault(x => x.Path == Model.InternalState.SelectedItemBeforeExpanding.Path);
            }

            if (selectedItem == null)
            {
                selectedItem = ExplorerItems.FirstOrDefault();
            }

            SelectedItem = selectedItem;
            Model.InternalState.SelectedItemBeforeExpanding = null;
        }
    }
}