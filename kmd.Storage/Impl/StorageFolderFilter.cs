using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Impl
{
    public class StorageFolderFilter : IStorageFolderFilter
    {
        public async Task<IEnumerable<IStorageItem2>> FilterAsync(IStorageFolder folder, FilterOptions options, CancellationToken cancellationToken = default(CancellationToken))
        {
            var items = await folder.GetItemsAsync();
            var filteredItems = new List<IStorageItem2>();
            foreach (var item in items.Where(x => x.Name.StartsWith(options.QueryText, StringComparison.OrdinalIgnoreCase)))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }
                filteredItems.Add(item as IStorageItem2);
            }
            return filteredItems;
        }
    }
}