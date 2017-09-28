using kdm.Core.Explorer.Commands.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kmd.Core.Explorer.Contracts;
using kmd.Storage.Contracts;
using System.Collections.ObjectModel;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand]
    public class FilterCommand : ExplorerCommand
    {
        protected readonly IStorageFolderFilter _storageFolderFilter;

        public FilterCommand(IStorageFolderFilter storageFolderFilter)
        {
            _storageFolderFilter = storageFolderFilter ?? throw new ArgumentNullException(nameof(storageFolderFilter));
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            Model.ViewState.IsBusy = true;

            var filteredItems = await _storageFolderFilter.FilterAsync(Model.ViewState.CurrentFolder,
                Model.InternalState.FilterOptions, Model.InternalState.CancellationTokenSource.Token);

            Model.ViewState.ExplorerItems = new ObservableCollection<IExplorerItem>(filteredItems);

            Model.ViewState.IsBusy = false;
        }
    }
}