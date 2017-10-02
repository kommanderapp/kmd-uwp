using kmd.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Commands.Configuration;
using kmd.Core.Services.Contracts;
using kmd.Core.Hotkeys;
using kmd.Storage.Extensions;
using System;
using Windows.Storage;
using Windows.System;

namespace kmd.Core.Explorer.Commands
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.Enter)]
    public class PasteToCurrentFolderCommand : ExplorerCommandBase
    {
        public PasteToCurrentFolderCommand(ICilpboardService cilpboardService)
        {
            _clipboardService = cilpboardService ?? throw new ArgumentNullException(nameof(_clipboardService));
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            ViewModel.IsBusy = true;

            var pastedItem = _clipboardService.Get();
            var storageItems = await pastedItem.GetStorageItemsAsync();
            var changesMade = false;
            foreach (var item in storageItems)
            {
                if (item is IStorageFolder)
                {
                    await (item as IStorageFolder).CopyContentsRecursiveAsync(ViewModel.CurrentFolder, ViewModel.CancellationTokenSource.Token);
                    changesMade = true;
                }
                else if (item is IStorageFile)
                {
                    await (item as IStorageFile).CopyAsync(ViewModel.CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                // if changed made refresh view
                await ViewModel.GoToAsync(ViewModel.CurrentFolder);
            }

            ViewModel.IsBusy = false;
        }

        protected readonly ICilpboardService _clipboardService;
    }
}