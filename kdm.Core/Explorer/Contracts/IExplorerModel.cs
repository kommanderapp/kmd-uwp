using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Explorer.Contracts
{
    public interface IExplorerModel
    {
        IExplorerViewState ViewState { get; }
        IExplorerInternalState InternalState { get; }

        Task RefreshAsync();

        Task GoToAsync(IStorageFolder folder);
    }
}