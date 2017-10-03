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
            return true;
        }

        protected override async void OnExecute(IExplorerViewModel vm)
        {
            var selectedItem = vm.SelectedItem;
            if (selectedItem != null && selectedItem.IsPhysical)
            {
                await _dialogService.ShowMessage("Explorer_DeleteFile_Message".GetLocalized(),
                    "Explorer_DeleteFile_Title".GetLocalized(),
                    "Explorer_DeleteFile_ConfirmButtonText".GetLocalized(),
                    "Explorer_DeleteFile_CancelButtonText".GetLocalized(),
                    async (accepted) =>
                    {
                        if (accepted)
                        {
                            await selectedItem.StorageItem.DeleteAsync();
                            vm.ExplorerItems.Remove(selectedItem);
                        }
                    }
             );
            }
        }
    }
}