using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Impl
{
    public class StorageFolderRootsExpander : IStorageFolderRootsExpander
    {
        public async Task<IEnumerable<IStorageFolder>> ExpandOuterAsync(IStorageFolder folder, CancellationToken token = default(CancellationToken))
        {
            var parents = new List<IStorageFolder>
            {
                folder
            };
            var parent = await ((IStorageItem2)folder).GetParentAsync();
            while (parent != null)
            {
                parents.Add(parent);

                if (token.IsCancellationRequested)
                {
                    break;
                }

                parent = await parent.GetParentAsync();
            }

            parents.Reverse();

            return parents;
        }
    }
}