using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Core.Hotkeys;
using kmd.Storage.Extensions;
using System;
using Windows.Storage;
using Windows.System;
using kmd.Core.Command;
using kmd.Core.Explorer.Contracts;

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
            return true;
        }

        protected override async void OnExecute(IExplorerViewModel vm)
        {
            var pastedItem = _clipboardService.Get();
            var storageItems = await pastedItem.GetStorageItemsAsync();
            var changesMade = false;
            foreach (var item in storageItems)
            {
                if (item is IStorageFolder)
                {
                    await (item as IStorageFolder).CopyContentsRecursiveAsync(vm.CurrentFolder, vm.CancellationTokenSource.Token);
                    changesMade = true;
                }
                else if (item is IStorageFile)
                {
                    await (item as IStorageFile).CopyAsync(vm.CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                    changesMade = true;
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