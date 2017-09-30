using kmd.Storage.Contracts;
using System.Collections.ObjectModel;
using Windows.Storage;

namespace kmd.Core.Explorer.Contracts
{
    public interface IExplorerViewState
    {
        bool IsPathBoxFocused { get; set; }
        bool IsBusy { get; set; }
        ObservableCollection<IExplorerItem> ExplorerItems { get; set; }
        IStorageFolder CurrentFolder { get; set; }
        ObservableCollection<IStorageFolder> CurrentFolderExpandedRoots { get; set; }
        IExplorerItem SelectedItem { get; set; }
        ObservableCollection<IExplorerItem> SelectedItems { get; set; }
    }
}