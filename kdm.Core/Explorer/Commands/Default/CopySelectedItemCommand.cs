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
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.Escape)]
    public class CopySelectedItemCommand : ExplorerCommand
    {
        protected readonly ICilpboardService _clipboardService;

        public CopySelectedItemCommand(ICilpboardService cilpboardService)
        {
            _clipboardService = cilpboardService ?? throw new ArgumentNullException(nameof(cilpboardService));
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            if (Model.ViewState.SelectedItem != null && Model.ViewState.SelectedItem.IsPhysical)
            {
                var dataObject = new DataPackage
                {
                    RequestedOperation = DataPackageOperation.Copy
                };
                dataObject.SetStorageItems(new List<IStorageItem>() { Model.ViewState.SelectedItem.StorageItem });
                _clipboardService.Set(dataObject);
            }
            await Task.FromResult(0);
        }
    }
}