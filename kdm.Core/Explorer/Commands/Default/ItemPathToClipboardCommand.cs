using kdm.Core.Explorer.Commands.Abstractions;
using kdm.Core.Services.Contracts;
using kmd.Core.Hotkeys;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.System;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.Enter)]
    public class ItemPathToClipboardCommand : ExplorerCommand
    {
        protected readonly ICilpboardService _cilpboardService;

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
            var selectedItem = Model.ViewState.SelectedItem;
            if (selectedItem != null && selectedItem.IsPhysical)
            {
                var data = new DataPackage();
                data.SetText(Model.ViewState.SelectedItem.Path);
                _cilpboardService.Set(data);
            }

            await Task.FromResult(0);
        }
    }
}