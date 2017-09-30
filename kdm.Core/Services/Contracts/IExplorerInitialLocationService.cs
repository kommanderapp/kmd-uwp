using System.Threading.Tasks;
using Windows.Storage;

namespace kdm.Core.Services.Contracts
{
    public interface IExplorerLocationService
    {
        Task<IStorageFolder> GtoToLocationAsync(string path);

        Task<IStorageFolder> PickInitialLocationAsync();
    }
}