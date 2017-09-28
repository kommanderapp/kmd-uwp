using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Contracts
{
    public interface IStorageFolderExpander
    {
        Task<IEnumerable<IExplorerItem>> ExpandInnerAsync(IStorageFolder folder, CancellationToken token = default(CancellationToken));

        Task<IEnumerable<IStorageFolder>> ExpandOuterAsync(IStorageFolder folder, CancellationToken token = default(CancellationToken));
    }
}