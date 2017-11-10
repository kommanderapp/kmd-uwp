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
    [ExplorerCommand("GroupRenameCommand", "Group rename", key: VirtualKey.R, modifierKey: ModifierKeys.Control)]
    public class GroupRenameCommand : ExplorerCommandBase
    {
        public GroupRenameCommand(IDialogService dialogService)
        {
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
        }

        protected readonly IDialogService _dialogService;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            return vm.CanGroup;
        }

        protected async override void OnExecuteAsync(IExplorerViewModel vm)
        {
            var name = await _dialogService.Prompt("Enter name", "group name");

            if (name == null) return;

            var itemIndex = 1;
            var count = vm.SelectedItems.Count;

            for (int i = 0; i < count; i++)
            {
                var item = vm.SelectedItems[0];
                try
                {
                    await item.StorageItem.RenameAsync($"{name} ({itemIndex}){item.FileType}");
                    vm.ExplorerItems.Remove(item);
                    vm.ExplorerItems.Add(await ExplorerItem.CreateAsync(item.StorageItem));
                }
                catch
                {
                    var currentName = $"{name} ({itemIndex})";

                    var result = await _dialogService.NameCollisionDialog(currentName);

                    if (result == Controls.ContentDialogs.NameCollisionDialogResult.Replace)
                    {
                        var existingItem = vm.ExplorerItems.First(it => it.Name == currentName + item.FileType);
                        await existingItem.StorageItem.DeleteAsync(Windows.Storage.StorageDeleteOption.Default);
                        vm.ExplorerItems.Remove(existingItem);

                        await item.StorageItem.RenameAsync(currentName + item.FileType);
                        vm.ExplorerItems.Add(await ExplorerItem.CreateAsync(item.StorageItem));
                    }
                    else if (result == Controls.ContentDialogs.NameCollisionDialogResult.Rename)
                    {
                        var items = await vm.CurrentFolder.GetItemsAsync();
                        currentName = NameCollision.GetUniqueNameForFile(currentName, item.FileType, items);

                        vm.ExplorerItems.Remove(item);
                        await item.StorageItem.RenameAsync(currentName);
                        vm.ExplorerItems.Add(await ExplorerItem.CreateAsync(item.StorageItem));
                    }
                }

                itemIndex++;
            }
        }
    }
}
