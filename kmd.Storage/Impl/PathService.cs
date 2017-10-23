using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Impl
{
    public class PathService : IPathService
    {
        public async Task<IEnumerable<IStorageFolder>> ExpandParents(IStorageFolder folder)
        {
            var parents = new List<IStorageFolder>
            {
                folder
            };
            var parent = await ((IStorageItem2)folder).GetParentAsync();
            while (parent != null)
            {
                parents.Add(parent);
                parent = await ((IStorageItem2)parent).GetParentAsync();
            }

            parents.Reverse();

            return parents;
        }

        public async Task<IStorageFolder> GetRootAsync(IStorageFolder folder)
        {
            var storageFolder = folder as IStorageItem2;

            IStorageItem2 parent = null;
            IStorageItem2 testParent = await storageFolder.GetParentAsync();

            while (testParent != null)
            {
                testParent = await testParent.GetParentAsync();
                if (testParent != null)
                {
                    parent = testParent;
                }
            }

            return parent as IStorageFolder;
        }

        public async Task<IStorageFolder> GoToAsync(IStorageFolder root, string path)
        {
            IStorageFolder resultFolder = root;
            string[] dirs = path.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar });

            if (dirs[0] != root.Name)
            {
                throw new InvalidOperationException($"Wrong root {root.Name} given for path {path}.");
            }

            for (int i = 1; i < dirs.Length; i++)
            {
                resultFolder = await resultFolder.GetFolderAsync(dirs[i]);
            }

            return resultFolder;
        }
    }
}