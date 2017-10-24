using GalaSoft.MvvmLight.Views;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Models;
using kmd.Core.Hotkeys;
using kmd.Core.Services.Contracts;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.F, modifierKey: ModifierKeys.Control)]
    public class AddNewFolderCommand : ExplorerCommandBase
    {
        public AddNewFolderCommand(IPromptService cusomDialogService, IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _promptService = cusomDialogService ?? throw new ArgumentNullException(nameof(cusomDialogService));
        }

        protected readonly IPromptService _promptService;
        protected readonly IDialogService _dialogService;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return true;
        }

        protected async override void OnExecuteAsync(IExplorerViewModel vm)
        {
            var folderName = await _promptService.Prompt("Enter folder name", "Create", "New folder");

            if (folderName == null) return;

            try
            {
                var folder = await vm.CurrentFolder.CreateFolderAsync(folderName);
                var newItem = await ExplorerItem.CreateAsync(folder);
                vm.ExplorerItems.Add(newItem);
                vm.SelectedItem = newItem;
            }
            catch (Exception e)
            {
                await _dialogService.ShowError(e, "Invalid operation", "Ok", null);
            }
        }
    }
}
