using GalaSoft.MvvmLight.Views;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Models;
using kmd.Core.Extensions;
using kmd.Core.Helpers;
using kmd.Core.Hotkeys;
using System;
using System.Linq;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand("RenameSelectedItem", "RenameSelectedItem", ModifierKeys.None, VirtualKey.F2)]
    public class RenameSelectedItemCommand : ExplorerCommandBase
    {
        public RenameSelectedItemCommand(IDialogService dialogService)
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
            var result = await _dialogService.Prompt("Rename file", vm.SelectedItem.Name);

            if (result == null) return;

            try
            {
                await vm.SelectedItem.StorageItem.RenameAsync(result);
                var storageItem = vm.SelectedItem.StorageItem;
                var newItem = await ExplorerItem.CreateAsync(storageItem);

                vm.ExplorerItems.Remove(vm.SelectedItem);
                vm.ExplorerItems.Add(newItem);
                vm.SelectedItem = newItem;
            }
            catch (Exception)
            {
                var unqiueName = string.Empty;

                if (vm.SelectedItem.IsFile)
                    unqiueName = NameCollision.GetUniqueNameForFile(result.Split('.').First(), vm.SelectedItem.FileType, await vm.CurrentFolder.GetItemsAsync());

                if (vm.SelectedItem.IsFolder)
                    unqiueName = NameCollision.GetUniqueNameForFolder(result, await vm.CurrentFolder.GetItemsAsync());

                var rename = await _dialogService.ShowMessage("There is already a file with the same name in this location.", $"Do you want to rename \"{vm.SelectedItem.Name}\" to \"{unqiueName}\"", "Ok", "Cancel", null);

                if (rename == true)
                {
                    await vm.SelectedItem.StorageItem.RenameAsync(unqiueName);
                    var storageItem = vm.SelectedItem.StorageItem;
                    var newItem = await ExplorerItem.CreateAsync(storageItem);

                    vm.ExplorerItems.Remove(vm.SelectedItem);
                    vm.ExplorerItems.Add(newItem);
                    vm.SelectedItem = newItem;
                }
            }
        }
    }
}
