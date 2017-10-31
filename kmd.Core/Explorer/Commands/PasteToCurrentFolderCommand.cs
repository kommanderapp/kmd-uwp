using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Explorer.Contracts;
using kmd.Core.Explorer.Models;
using kmd.Core.Helpers;
using kmd.Core.Hotkeys;
using kmd.Core.Services.Contracts;
using kmd.Storage.Extensions;
using System;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.V)]
    public class PasteToCurrentFolderCommand : ExplorerCommandBase
    {
        public PasteToCurrentFolderCommand(ICilpboardService cilpboardService, ICustomDialogService customDialogService, NavigateCommand navigateCommand)
        {
            _clipboardService = cilpboardService ?? throw new ArgumentNullException(nameof(cilpboardService));
            _customDialogService = customDialogService ?? throw new ArgumentNullException(nameof(customDialogService));
            _navigateCommand = navigateCommand ?? throw new ArgumentNullException(nameof(navigateCommand));
        }

        protected readonly ICilpboardService _clipboardService;
        protected readonly ICustomDialogService _customDialogService;
        protected readonly NavigateCommand _navigateCommand;

        protected override bool OnCanExecute(IExplorerViewModel vm)
        {
            var pastedItem = _clipboardService.Get();
            if (pastedItem.Contains(StandardDataFormats.StorageItems))
            {
                return true;
            }
            return false;
        }

        protected override async void OnExecuteAsync(IExplorerViewModel vm)
        {
            var pastedItem = _clipboardService.Get();
            var storageItems = await pastedItem.GetStorageItemsAsync();
            var changesMade = false;
            foreach (var item in storageItems)
            {
                if (item is IStorageFolder)
                {
                    try
                    {
                        var folder = await vm.CurrentFolder.CreateFolderAsync((item as IStorageFolder).Name);
                        await (item as IStorageFolder).CopyContentsRecursiveAsync(folder, vm.CancellationTokenSource.Token);
                        changesMade = true;
                    }
                    catch
                    {
                        var result = await _customDialogService.NameCollisionDialog(item.Name);

                        if (result == Controls.ContentDialogs.NameCollisionDialogResult.Replace)
                        {
                            var existingItem = vm.ExplorerItems.First(i => i.Name == (item as IStorageFolder).Name);
                            await existingItem.StorageItem.DeleteAsync(StorageDeleteOption.Default);
                            vm.ExplorerItems.Remove(existingItem);

                            var folder = await vm.CurrentFolder.CreateFolderAsync((item as IStorageFolder).Name);
                            vm.ExplorerItems.Add(await ExplorerItem.CreateAsync(folder));
                            changesMade = true;
                        }
                        else if (result == Controls.ContentDialogs.NameCollisionDialogResult.Rename)
                        {
                            var folderName = (item as IStorageFolder).Name;
                            var items = await vm.CurrentFolder.GetItemsAsync();

                            var folder = await vm.CurrentFolder.CreateFolderAsync(NameCollision.GetUniqueNameForFolder(folderName, items));
                            vm.ExplorerItems.Add(await ExplorerItem.CreateAsync(folder));
                            await (item as IStorageFolder).CopyContentsRecursiveAsync(folder, vm.CancellationTokenSource.Token);
                            changesMade = true;
                        }
                    }
                }
                else if (item is IStorageFile)
                {
                    try
                    {
                        await (item as IStorageFile).CopyAsync(vm.CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                        changesMade = true;
                    }
                    catch
                    {
                        var result = await _customDialogService.NameCollisionDialog(item.Name);

                        if (result == Controls.ContentDialogs.NameCollisionDialogResult.Replace)
                        {
                            await (item as IStorageFile).CopyAsync(vm.CurrentFolder, item.Name, NameCollisionOption.ReplaceExisting);
                            changesMade = true;
                        }
                        else if (result == Controls.ContentDialogs.NameCollisionDialogResult.Rename)
                        {
                            await (item as IStorageFile).CopyAsync(vm.CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                            changesMade = true;
                        }
                    }
                }
            }

            if (changesMade)
            {
                // if changes made refresh view
                vm.CurrentFolder = vm.CurrentFolder;
            }
        }
    }
}