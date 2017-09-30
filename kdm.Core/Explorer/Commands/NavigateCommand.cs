using kdm.Core.Explorer.Commands.Abstractions;
using kdm.Core.Explorer.Commands.Configuration;
using kmd.Storage.Contracts;
using System;
using Windows.Storage;

namespace kdm.Core.Explorer.Commands
{
    [ExplorerCommand]
    public class NavigateCommand : ExplorerCommandBase
    {
        protected readonly IStorageFolderExploder _storageFolderExpander;
        protected readonly IStorageFolderLister _storageFolderLister;

        public NavigateCommand(IStorageFolderExploder storageFolderExpander, IStorageFolderLister storageFolderLister)
        {
            _storageFolderExpander = storageFolderExpander ?? throw new ArgumentNullException(nameof(storageFolderExpander));
            _storageFolderLister = storageFolderLister ?? throw new ArgumentNullException(nameof(storageFolderLister));
        }

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override async void Execute(object parameter)
        {
            await ViewModel.GoToAsync(parameter as IStorageFolder);
        }
    }
}