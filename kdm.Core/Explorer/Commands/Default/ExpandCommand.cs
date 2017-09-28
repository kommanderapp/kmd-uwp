using kdm.Core.Explorer.Commands.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Hotkeys;
using Windows.System;
using kmd.Storage.Contracts;
using System.Collections.ObjectModel;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.B)]
    public class ExpandCommand : ExplorerCommand
    {
        protected readonly IStorageFolderExpander _storageFolderExpander;

        public ExpandCommand(IStorageFolderExpander storageFolderExpander)
        {
            _storageFolderExpander = storageFolderExpander ?? throw new ArgumentNullException(nameof(storageFolderExpander));
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            Model.ViewState.IsBusy = true;

            var items = await _storageFolderExpander.ExpandInnerAsync(Model.ViewState.CurrentFolder, Model.InternalState.CancellationTokenSource.Token);

            Model.InternalState.ItemsState = ExplorerItemsStates.Expanded;
            Model.InternalState.SelectedItemBeforeExpanding = Model.ViewState.SelectedItem;

            Model.ViewState.ExplorerItems = new ObservableCollection<IExplorerItem>(items);

            Model.ViewState.IsBusy = false;
        }
    }
}