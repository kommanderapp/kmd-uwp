using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Contracts
{
    public interface IStorageFolderLister
    {
        Task<IEnumerable<IStorageItem2>> ListAsync(IStorageFolder folder, CancellationToken cancellationToken = default(CancellationToken));
    }
}