using kdm.Core.Explorer.Commands.Abstractions;
using kdm.Core.Services.Contracts;
using kmd.Core.Hotkeys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.System;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.X)]
    internal class CutSelectedItemCommand : ExplorerCommand
    {
        protected readonly ICilpboardService _clipboardService;

        public CutSelectedItemCommand(ICilpboardService cilpboardService)
        {
            _clipboardService = cilpboardService ?? throw new ArgumentNullException(nameof(cilpboardService));
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            var selectedItem = Model.ViewState.SelectedItem;
            if (selectedItem != null && selectedItem.IsPhysical)
            {
                var dataObject = new DataPackage
                {
                    RequestedOperation = DataPackageOperation.Move
                };
                dataObject.SetStorageItems(new List<IStorageItem>() { selectedItem.StorageItem });
                _clipboardService.Set(dataObject);
            }

            await Task.FromResult(0);
        }
    }
}