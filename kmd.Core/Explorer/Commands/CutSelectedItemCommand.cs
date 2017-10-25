using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Hotkeys;
using kmd.Core.Services.Contracts;
using System;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.X)]
    public class CutSelectedItemCommand : ExplorerCommandBase
    {
        public CutSelectedItemCommand(ICilpboardService cilpboardService)
        {
            _clipboardService = cilpboardService ?? throw new ArgumentNullException(nameof(cilpboardService));
        }

        protected readonly ICilpboardService _clipboardService;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.SelectedItem != null && vm.SelectedItem.IsPhysical;
        }

        protected override void OnExecuteAsync(IExplorerViewModel vm)
        {
            var dataObject = new DataPackage
            {
                RequestedOperation = DataPackageOperation.Move
            };
            dataObject.SetStorageItems(vm.SelectedItems.Select(i => i.StorageItem));
            _clipboardService.Set(dataObject);
        }
    }
}