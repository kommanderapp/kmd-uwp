using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Core.Services.Contracts
{
    public interface ILocationService
    {
        Task<IEnumerable<IStorageFolder>> GetLocationsAsync();

        Task<IStorageFolder> PickLocationAsync();

        Task RemoveLocationAsync(IStorageFolder folder);
    }
}