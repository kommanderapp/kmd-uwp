using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Windows.Storage;

namespace kmd.Core.Explorer.Contracts
{
    public interface IExplorerViewState
    {
        bool IsBusy { get; set; }
        ObservableCollection<IExplorerItem> ExplorerItems { get; set; }
        IStorageFolder CurrentFolder { get; set; }
        ObservableCollection<IStorageFolder> CurrentFolderExpandedRoots { get; set; }
        IExplorerItem SelectedItem { get; set; }
        ObservableCollection<IExplorerItem> SelectedItems { get; set; }
    }
}