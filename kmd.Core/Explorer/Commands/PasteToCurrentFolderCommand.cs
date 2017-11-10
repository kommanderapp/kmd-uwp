using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Core.Hotkeys;
using kmd.Storage.Extensions;
using System;
using Windows.Storage;
using Windows.System;
using kmd.Core.Explorer.Contracts;
using Windows.ApplicationModel.DataTransfer;
using kmd.Core.Explorer.Models;
using GalaSoft.MvvmLight.Views;
using kmd.Core.Extensions;
using System.Linq;
using kmd.Core.Helpers;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand("PasteToCurrentFolder", "Paste to current folder", ModifierKeys.Control, VirtualKey.V)]
    public class PasteToCurrentFolderCommand : ExplorerCommandBase
    {
        public PasteToCurrentFolderCommand(ICilpboardService cilpboardService, IDialogService dialogService, NavigateCommand navigateCommand)
        {
            _clipboardService = cilpboardService ?? throw new ArgumentNullException(nameof(cilpboardService));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _navigateCommand = navigateCommand ?? throw new ArgumentNullException(nameof(navigateCommand));
        }

        protected readonly ICilpboardService _clipboardService;
        protected readonly IDialogService _dialogService;
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

            foreach (var item in storageItems)
            {
                if (item is IStorageFolder)
                {
                    try
                    {
                        var folder = await vm.CurrentFolder.CreateFolderAsync((item as IStorageFolder).Name);
                        await (item as IStorageFolder).CopyContentsRecursiveAsync(folder, vm.CancellationTokenSource.Token);
                        var explorerItem = await ExplorerItem.CreateAsync(folder);
                        vm.ExplorerItems.Add(explorerItem);
                    }
                    catch
                    {
                        var result = await _dialogService.NameCollisionDialog(item.Name);

                        if (result == Controls.ContentDialogs.NameCollisionDialogResult.Replace)
                        {
                            var existingItem = vm.ExplorerItems.First(i => i.Name == (item as IStorageFolder).Name);
                            await existingItem.StorageItem.DeleteAsync(StorageDeleteOption.Default);
                            vm.ExplorerItems.Remove(existingItem);

                            var folder = await vm.CurrentFolder.CreateFolderAsync((item as IStorageFolder).Name);
                            vm.ExplorerItems.Add(await ExplorerItem.CreateAsync(folder));
                        }
                        else if (result == Controls.ContentDialogs.NameCollisionDialogResult.Rename)
                        {
                            var folderName = (item as IStorageFolder).Name;
                            var items = await vm.CurrentFolder.GetItemsAsync();

                            var folder = await vm.CurrentFolder.CreateFolderAsync(NameCollision.GetUniqueNameForFolder(folderName, items, " - Copy"));
                            vm.ExplorerItems.Add(await ExplorerItem.CreateAsync(folder));
                            await (item as IStorageFolder).CopyContentsRecursiveAsync(folder, vm.CancellationTokenSource.Token);
                        }
                    }

                }
                else if (item is IStorageFile)
                {
                    try
                    {
                        var file = await (item as IStorageFile).CopyAsync(vm.CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                        var explorerItem = await ExplorerItem.CreateAsync(file);
                        vm.ExplorerItems.Add(explorerItem);
                    }
                    catch
                    {
                        var result = await _dialogService.NameCollisionDialog(item.Name);

                        if (result == Controls.ContentDialogs.NameCollisionDialogResult.Replace)
                        {
                            await (item as IStorageFile).CopyAsync(vm.CurrentFolder, item.Name, NameCollisionOption.ReplaceExisting);
                        }
                        else if (result == Controls.ContentDialogs.NameCollisionDialogResult.Rename)
                        {
                            await (item as IStorageFile).CopyAsync(vm.CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                        }
                    }
                }
            }
        }
    }
}
