using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Core.Hotkeys;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.X)]
    internal class CutSelectedItemCommand : ExplorerCommandBase
    {
        public CutSelectedItemCommand(ICilpboardService cilpboardService)
        {
            _clipboardService = cilpboardService ?? throw new ArgumentNullException(nameof(cilpboardService));
        }

        protected readonly ICilpboardService _clipboardService;

        protected override bool OnCanExecute(object parameter)
        {
            return true;
        }

        protected override async void OnExecute(object parameter)
        {
            var selectedItem = ViewModel.SelectedItem;
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