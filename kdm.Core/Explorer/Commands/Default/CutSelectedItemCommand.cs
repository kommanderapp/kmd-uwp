using kdm.Core.Explorer.Commands.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kmd.Core.Explorer.Contracts;
using kdm.Core.Services.Contracts;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.System;
using kmd.Core.Explorer.Hotkeys;

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