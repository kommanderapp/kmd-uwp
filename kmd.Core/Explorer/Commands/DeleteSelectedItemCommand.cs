using GalaSoft.MvvmLight.Views;
using kmd.Core.Command;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Helpers;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.Delete)]
    public class DeleteSelectedItemCommand : ExplorerCommandBase
    {
        public DeleteSelectedItemCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        protected readonly IDialogService _dialogService;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.SelectedItem != null && vm.SelectedItem.IsPhysical;
        }

        protected override async void OnExecuteAsync(IExplorerViewModel vm)
        {
            await _dialogService.ShowMessage("Explorer_DeleteFile_Message".GetLocalized(),
                "Explorer_DeleteFile_Title".GetLocalized(),
                "Explorer_DeleteFile_ConfirmButtonText".GetLocalized(),
                "Explorer_DeleteFile_CancelButtonText".GetLocalized(),
                async (accepted) =>
                {
                    if (accepted)
                    {
                        await vm.SelectedItem.StorageItem.DeleteAsync();
                        vm.ExplorerItems.Remove(vm.SelectedItem);
                    }
                }
         );
        }
    }
}