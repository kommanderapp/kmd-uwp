using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Windows.Storage;

namespace kmd.Storage.Impl
{
    public class StorageFolderFilter : IStorageFolderFilter
    {
        public async Task<IEnumerable<IExplorerItem>> FilterAsync(IStorageFolder folder, FilterOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await folder.GetItemsAsync();
            var filteredItems = new List<IExplorerItem>();
            foreach (var item in items.Where(x => x.Name.StartsWith(options.QueryText, StringComparison.OrdinalIgnoreCase)))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                var explorerItem = await ExplorerItem.CreateAsync(item as IStorageItem2);
                filteredItems.Add(explorerItem);
            }
            return filteredItems;
        }
    }
}