using GalaSoft.MvvmLight.Views;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Hotkeys;
using kmd.Core.Services.Contracts;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.R, modifierKey: ModifierKeys.Control)]
    public class GroupRenameCommand : ExplorerCommandBase
    {
        public GroupRenameCommand(IPromptService cusomDialogService, IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _promptService = cusomDialogService ?? throw new ArgumentNullException(nameof(cusomDialogService));
        }

        protected readonly IPromptService _promptService;
        protected readonly IDialogService _dialogService;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.CanGroup;
        }

        protected async override void OnExecuteAsync(IExplorerViewModel vm)
        {
            var name = await _promptService.Prompt("Enter name", "Continue", "group name");

            if (name == null) return;

            try
            {
                var itemIndex = 1;

                foreach (var item in vm.SelectedItems)
                {
                    await item.StorageItem.RenameAsync($"{name} ({itemIndex}){item.FileType}");
                    itemIndex++;
                }

                vm.CurrentFolder = vm.CurrentFolder;
            }
            catch (Exception e)
            {
                await _dialogService.ShowError(e, "Invalid operation", "Ok", null);
            }
        }
    }
}