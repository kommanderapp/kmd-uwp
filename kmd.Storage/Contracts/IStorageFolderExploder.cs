using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Contracts
{
    public interface IStorageFolderExploder
    {
        Task<IEnumerable<IStorageItem2>> ExplodeAsync(IStorageFolder folder, CancellationToken token = default(CancellationToken));
    }
}