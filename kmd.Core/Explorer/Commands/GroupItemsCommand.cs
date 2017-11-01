using GalaSoft.MvvmLight.Views;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Models;
using kmd.Core.Extensions;
using kmd.Core.Hotkeys;
using kmd.Storage.Extensions;
using System;
using Windows.Storage;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(key: VirtualKey.Q, modifierKey: ModifierKeys.Control)]
    public class GroupItemsCommand : ExplorerCommandBase
    {
        public GroupItemsCommand(IDialogService dialogService)
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
            var name = await _dialogService.Prompt("Enter folder name", "Group folder");

            if (name == null) return;

            try
            {
                var result = await vm.CurrentFolder.CreateFolderAsync(name);
                var newItem = await ExplorerItem.CreateAsync(result);
                vm.ExplorerItems.Add(newItem);

                var count = vm.SelectedItems.Count;
                for (int i = 0; i < count; i++)
                {
                    if (vm.SelectedItems[i].IsFile)
                    {
                        await (vm.SelectedItems[i].StorageItem as IStorageFile).CopyAsync(newItem.AsFolder, vm.SelectedItems[i].Name, NameCollisionOption.GenerateUniqueName);
                    }
                    else if (vm.SelectedItems[i].IsFolder)
                    {
                        var folder = await newItem.AsFolder.CreateFolderAsync((vm.SelectedItems[i].StorageItem as IStorageFolder).Name);
                        await (vm.SelectedItems[i].StorageItem as IStorageFolder).CopyContentsRecursiveAsync(folder, vm.CancellationTokenSource.Token);
                    }

                    await vm.SelectedItems[i].StorageItem.DeleteAsync();
                }
                for (int i = 0; i < count; i++)
                {
                    vm.ExplorerItems.Remove(vm.SelectedItems[0]);
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
