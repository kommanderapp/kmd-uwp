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
    [ExplorerCommand("AddNewFolder", "Add new folder", key: VirtualKey.F, modifierKey: ModifierKeys.Control)]
    public class AddNewFolderCommand : ExplorerCommandBase
    {
        public AddNewFolderCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }
        
        protected readonly IDialogService _dialogService;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return true;
        }

        protected override async void OnExecuteAsync(IExplorerViewModel vm)
        {
            var folderName = await _dialogService.Prompt("Enter folder name", "New folder");

            if (folderName == null) return;

            try
            {
                var folder = await vm.CurrentFolder.CreateFolderAsync(folderName);
                var newItem = await ExplorerItem.CreateAsync(folder);
                vm.ExplorerItems.Add(newItem);
                vm.SelectedItem = newItem;
            }
            catch
            {
                var result = await _dialogService.NameCollisionDialog(folderName);

                if (result == Controls.ContentDialogs.NameCollisionDialogResult.Replace)
                {
                    var item = vm.ExplorerItems.First(i => i.Name == folderName);
                    await item.StorageItem.DeleteAsync(Windows.Storage.StorageDeleteOption.Default);
                    vm.ExplorerItems.Remove(item);

                    var folder = await vm.CurrentFolder.CreateFolderAsync(folderName);
                    var newItem = await ExplorerItem.CreateAsync(folder);
                    vm.ExplorerItems.Add(newItem);
                    vm.SelectedItem = newItem;
                }
                else if (result == Controls.ContentDialogs.NameCollisionDialogResult.Rename)
                {
                    var items = await vm.CurrentFolder.GetItemsAsync();
                    var unqiueName = NameCollision.GetUniqueNameForFolder(folderName, items);

                    var folder = await vm.CurrentFolder.CreateFolderAsync(unqiueName);
                    var newItem = await ExplorerItem.CreateAsync(folder);
                    vm.ExplorerItems.Add(newItem);
                    vm.SelectedItem = newItem;
                }
            }
        }
    }
}
