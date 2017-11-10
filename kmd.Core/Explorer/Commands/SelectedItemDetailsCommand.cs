using GalaSoft.MvvmLight.Views;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Extensions;
using kmd.Core.Hotkeys;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand("SelectedItemDetails", "Selected item details", ModifierKeys.None, VirtualKey.Space)]
    public class SelectedItemDetailsCommand : ExplorerCommandBase
    {
        public SelectedItemDetailsCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        protected readonly IDialogService _dialogService;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.SelectedItems.Count == 1 && vm.SelectedItem.IsPhysical;
        }
        protected override async void OnExecuteAsync(IExplorerViewModel vm)
        {
            await _dialogService.FileInfo(vm.SelectedItem);
        }
    }
}
