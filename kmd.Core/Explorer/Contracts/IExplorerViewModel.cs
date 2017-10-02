using kmd.Core.Commands.Abstractions;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Explorer.Contracts
{
    public interface IExplorerViewModel : IExplorerViewState, IExplorerInternalState, IViewModelWithCommands
    {
        Task GoToAsync(IStorageFolder folder);

        Task RefreshAsync();
    }
}