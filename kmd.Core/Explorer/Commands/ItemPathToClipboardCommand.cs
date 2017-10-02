using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Core.Hotkeys;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.Enter)]
    public class ItemPathToClipboardCommand : ExplorerCommandBase
    {
        public ItemPathToClipboardCommand(ICilpboardService cilpboardService)
        {
            _cilpboardService = cilpboardService ?? throw new ArgumentNullException(nameof(cilpboardService));
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            var selectedItem = ViewModel.SelectedItem;
            if (selectedItem != null && selectedItem.IsPhysical)
            {
                var data = new DataPackage();
                data.SetText(ViewModel.SelectedItem.Path);
                _cilpboardService.Set(data);
            }

            await Task.FromResult(0);
        }

        protected readonly ICilpboardService _cilpboardService;
    }
}