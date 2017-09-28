using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Contracts
{
    public class FilterOptions
    {
        public string QueryText { get; set; }
    }

    public interface IStorageFolderFilter
    {
        Task<IEnumerable<IExplorerItem>> FilterAsync(IStorageFolder folder, FilterOptions options, CancellationToken cancellationToken = default(CancellationToken));
    }
}