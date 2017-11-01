using System.Collections.ObjectModel;
using Windows.Storage;

namespace kmd.Core.Explorer.Contracts
{
    public interface IExplorerViewState
    {
        IStorageFolder CurrentFolder { get; set; }
        ObservableCollection<IStorageFolder> CurrentFolderExpandedRoots { get; set; }
        ObservableCollection<IExplorerItem> ExplorerItems { get; set; }
        bool IsBusy { get; set; }
        bool CanGroup { get; }
        bool IsPathBoxFocused { get; set; }
        IExplorerItem SelectedItem { get; set; }
        ObservableCollection<IExplorerItem> SelectedItems { get; set; }
    }
}