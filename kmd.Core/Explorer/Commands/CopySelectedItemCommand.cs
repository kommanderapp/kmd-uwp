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
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.Escape)]
    public class CopySelectedItemCommand : ExplorerCommandBase
    {
        public CopySelectedItemCommand(ICilpboardService cilpboardService)
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
            if (ViewModel.SelectedItem != null && ViewModel.SelectedItem.IsPhysical)
            {
                var dataObject = new DataPackage
                {
                    RequestedOperation = DataPackageOperation.Copy
                };
                dataObject.SetStorageItems(new List<IStorageItem>() { ViewModel.SelectedItem.StorageItem });
                _clipboardService.Set(dataObject);
            }
            await Task.FromResult(0);
        }
    }
}