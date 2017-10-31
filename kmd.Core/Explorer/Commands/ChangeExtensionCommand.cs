using GalaSoft.MvvmLight.Views;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Models;
using kmd.Core.Hotkeys;
using kmd.Core.Services.Contracts;
using System;
using System.Linq;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.E, modifierKey: ModifierKeys.Control)]
    public class ChangeExtensionCommand : ExplorerCommandBase
    {
        public ChangeExtensionCommand(ICustomDialogService cusomDialogService, IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _cusomDialogService = cusomDialogService ?? throw new ArgumentNullException(nameof(cusomDialogService));
        }

        protected readonly ICustomDialogService _cusomDialogService;
        protected readonly IDialogService _dialogService;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.SelectedItem.IsFile && vm.SelectedItems.Count == 1 && vm.SelectedItem.IsPhysical;
        }

        protected async override void OnExecuteAsync(IExplorerViewModel vm)
        {
            var result = await _cusomDialogService.Prompt("Change file extension", vm.SelectedItem.FileType);

            if (result == null) return;

            try
            {
                await vm.SelectedItem.StorageItem.RenameAsync(vm.SelectedItem.Name.Split('.').First() + result);
                var storageItem = vm.SelectedItem.StorageItem;
                var newItem = await ExplorerItem.CreateAsync(storageItem);

                vm.ExplorerItems.Remove(vm.SelectedItem);
                vm.ExplorerItems.Add(newItem);
                vm.SelectedItem = newItem;
            }
            catch (Exception ex)
            {
                await _dialogService.ShowError(ex.Message, "Invalid operation", "Ok", null);
            }
        }
    }
}
