using GalaSoft.MvvmLight.Views;
using kdm.Core.Explorer.Commands.Abstractions;
using kdm.Core.Explorer.Commands.Configuration;
using kmd.Core.Helpers;
using System;
using Windows.System;

namespace kdm.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.Delete)]
    public class DeleteSelectedItemCommand : ExplorerCommandBase
    {
        public DeleteSelectedItemCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
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
                await _dialogService.ShowMessage("Explorer_DeleteFile_Message".GetLocalized(),
                    "Explorer_DeleteFile_Title".GetLocalized(),
                    "Explorer_DeleteFile_ConfirmButtonText".GetLocalized(),
                    "Explorer_DeleteFile_CancelButtonText".GetLocalized(),
                    async (accepted) =>
                    {
                        if (accepted)
                        {
                            await selectedItem.StorageItem.DeleteAsync();
                            ViewModel.ExplorerItems.Remove(selectedItem);
                        }
                    }
             );
            }
        }

        protected readonly IDialogService _dialogService;
    }
}