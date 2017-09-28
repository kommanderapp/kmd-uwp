using kdm.Core.Explorer.Commands.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kmd.Core.Explorer.Contracts;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using kdm.Core.Services.Contracts;
using Windows.System;
using kmd.Core.Explorer.Hotkeys;

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