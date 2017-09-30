using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Impl
{
    public class StorageFolderExploder : IStorageFolderExploder
    {
        public async Task<IEnumerable<IStorageItem2>> ExplodeAsync(IStorageFolder folder, CancellationToken token)
        {
            var result = new List<IStorageItem2>();
            foreach (var item in await folder.GetItemsAsync())
            {
                if (!(item is IStorageItem2)) continue;

                if (token.IsCancellationRequested)
                {
                    break;
                }

                if (item.IsOfType(StorageItemTypes.File))
                {
                    result.Add((IStorageItem2)item);
                }
                else if (item.IsOfType(StorageItemTypes.Folder))
                {
                    var innerFolderItems = await ExplodeAsync((IStorageFolder)item, token);
                    result.AddRange(innerFolderItems);
                }
            }
            return result;
        }
    }
}