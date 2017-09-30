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
        public async Task<IEnumerable<IStorageItem2>> ListAsync(IStorageFolder folder, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (folder == null) throw new ArgumentNullException(nameof(folder));

            var result = new List<IStorageItem2>();

            foreach (var item in await folder.GetItemsAsync())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                if (!(item is IStorageItem2)) continue;
                result.Add((IStorageItem2)item);
            }

            return result;
        }
    }
}