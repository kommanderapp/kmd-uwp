using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Contracts
{
    public interface IStorageFolderRootsExpander
    {
        Task<IEnumerable<IStorageFolder>> ExpandOuterAsync(IStorageFolder folder, CancellationToken token = default(CancellationToken));
    }
}