using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using System.Threading;

namespace kmd.Storage.Impl
{
    public class StorageFolderExpander : IStorageFolderExpander
    {
        public async Task<IEnumerable<IExplorerItem>> ExpandInnerAsync(IStorageFolder folder, CancellationToken token)
        {
            var result = new List<IExplorerItem>();
            foreach (var item in await folder.GetItemsAsync())
            {
                if (!(item is IStorageItem2)) continue;

                if (token.IsCancellationRequested)
                {
                    break;
                }

                if (item.IsOfType(StorageItemTypes.File))
                {
                    var storageItem = await ExplorerItem.CreateAsync(item as IStorageItem2);
                    result.Add(storageItem);
                }
                else if (item.IsOfType(StorageItemTypes.Folder))
                {
                    var innerFolderItems = await ExpandInnerAsync((IStorageFolder)item, token);
                    result.AddRange(innerFolderItems);
                }
            }
            return result;
        }

        public async Task<IEnumerable<IStorageFolder>> ExpandOuterAsync(IStorageFolder folder, CancellationToken token = default(CancellationToken))
        {
            var parents = new List<IStorageFolder>();
            parents.Add(folder);
            var parent = await ((IStorageItem2)folder).GetParentAsync();
            while (parent != null)
            {
                parents.Add(parent);

                if (token.IsCancellationRequested)
                {
                    break;
                }

                parent = await ((IStorageItem2)parent).GetParentAsync();
            }

            parents.Reverse();

            return parents;
        }
    }
}