using kdm.Core.Explorer.Commands.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kmd.Core.Explorer.Contracts;
using System.Collections.ObjectModel;
using kmd.Storage.Contracts;
using Windows.Storage;

namespace kdm.Core.Explorer.Commands.Default
{
    [ExplorerCommand]
    public class NavigateCommand : ExplorerCommand
    {
        protected readonly IStorageFolderExpander _storageFolderExpander;
        protected readonly IStorageFolderLister _storageFolderLister;

        public NavigateCommand(IStorageFolderExpander storageFolderExpander, IStorageFolderLister storageFolderLister)
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
            await Model.GoToAsync(parameter as IStorageFolder);
        }
    }
}