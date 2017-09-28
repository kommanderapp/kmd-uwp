using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Impl
{
    public class StorageFolderLister : IStorageFolderLister
    {
        public async Task<IEnumerable<IExplorerItem>> ListAsync(IStorageFolder folder, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var result = new List<ExplorerItem>();

            foreach (var item in await folder.GetItemsAsync())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                if (!(item is IStorageItem2)) continue;
                var storageItem = await ExplorerItem.CreateAsync(item as IStorageItem2);
                result.Add(storageItem);
            }

            return result;
        }
    }
}