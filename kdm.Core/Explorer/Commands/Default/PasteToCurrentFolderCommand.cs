using kdm.Core.Explorer.Commands.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kmd.Core.Explorer.Contracts;
using kdm.Core.Services.Contracts;
using Windows.Storage;
using kmd.Storage.Extensions;
using kmd.Core.Explorer.Hotkeys;
using Windows.System;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand(modifierKey: ModifierKeys.Control, key: VirtualKey.Enter)]
    public class PasteToCurrentFolderCommand : ExplorerCommand
    {
        protected readonly ICilpboardService _clipboardService;

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
            Model.ViewState.IsBusy = true;

            var pastedItem = _clipboardService.Get();
            var storageItems = await pastedItem.GetStorageItemsAsync();
            var changesMade = false;
            foreach (var item in storageItems)
            {
                if (item is IStorageFolder)
                {
                    await (item as IStorageFolder).CopyContentsRecursiveAsync(Model.ViewState.CurrentFolder, Model.InternalState.CancellationTokenSource.Token);
                    changesMade = true;
                }
                else if (item is IStorageFile)
                {
                    await (item as IStorageFile).CopyAsync(Model.ViewState.CurrentFolder, item.Name, NameCollisionOption.GenerateUniqueName);
                    changesMade = true;
                }
            }

            if (changesMade)
            {
                // if changed made refresh view
                await Model.GoToAsync(Model.ViewState.CurrentFolder);
            }

            Model.ViewState.IsBusy = false;
        }
    }
}