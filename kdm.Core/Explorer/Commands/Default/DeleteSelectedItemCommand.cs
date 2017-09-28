using kdm.Core.Explorer.Commands.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kmd.Core.Explorer.Contracts;
using GalaSoft.MvvmLight.Views;
using kmd.Core.Helpers;
using Windows.System;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand(key: VirtualKey.Delete)]
    public class DeleteSelectedItemCommand : ExplorerCommand
    {
        protected readonly IDialogService _dialogService;

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
            var selectedItem = Model.ViewState.SelectedItem;
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
                            Model.ViewState.ExplorerItems.Remove(selectedItem);
                        }
                    }
             );
            }
        }
    }
}