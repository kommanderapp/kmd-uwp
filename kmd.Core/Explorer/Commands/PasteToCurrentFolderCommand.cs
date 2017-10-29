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

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.V)]
    public class PasteToCurrentFolderCommand : ExplorerCommandBase
    {
        public PasteToCurrentFolderCommand(ICilpboardService cilpboardService, NavigateCommand navigateCommand)
        {
            _clipboardService = cilpboardService ?? throw new ArgumentNullException(nameof(cilpboardService));
            _navigateCommand = navigateCommand ?? throw new ArgumentNullException(nameof(navigateCommand));
        }

        protected readonly ICilpboardService _clipboardService;
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
                    var folder = await vm.CurrentFolder.CreateFolderAsync((item as IStorageFolder).Name);
                    await (item as IStorageFolder).CopyContentsRecursiveAsync(folder, vm.CancellationTokenSource.Token);
                    var explorerItem = await ExplorerItem.CreateAsync(folder);
                    vm.ExplorerItems.Add(explorerItem);
                }
                else if (item is IStorageFile)
                {
                    var file = await (item as IStorageFile).CopyAsync(vm.CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                    var explorerItem = await ExplorerItem.CreateAsync(file);
                    vm.ExplorerItems.Add(explorerItem);
                }
            }
        }
    }
}