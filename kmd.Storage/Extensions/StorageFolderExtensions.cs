using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Extensions
{
    public static class StorageFolderExtensions
    {
        /// <summary>
        /// Recursive copy of files and folders from source to destination.
        /// </summary>
        public static async Task CopyContentsRecursiveAsync(this IStorageFolder source, IStorageFolder dest, CancellationToken token = default(CancellationToken))
        {
            await CopyContentsShallowAsync(source, dest, token);

            var subfolders = await source.GetFoldersAsync();
            foreach (var storageFolder in subfolders)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                await storageFolder.CopyContentsRecursiveAsync(await dest.GetFolderAsync(storageFolder.Name), token);
            }
        }

        /// <summary>
        /// Shallow copy of files and folders from source to destination.
        /// </summary>
        public static async Task CopyContentsShallowAsync(this IStorageFolder source, IStorageFolder destination, CancellationToken token = default(CancellationToken))
        {
            await source.CopyFilesAsync(destination, token);

            var items = await source.GetFoldersAsync();

            foreach (var storageFolder in items)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                await destination.CreateFolderAsync(storageFolder.Name, CreationCollisionOption.ReplaceExisting);
            }
        }

        /// <summary>
        /// Copy files from source into destination folder.
        /// </summary>
        private static async Task CopyFilesAsync(this IStorageFolder source, IStorageFolder destination, CancellationToken token = default(CancellationToken))
        {
            var items = await source.GetFilesAsync();

            foreach (var storageFile in items)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                await storageFile.CopyAsync(destination, storageFile.Name, NameCollisionOption.ReplaceExisting);
            }
        }

        /// <summary>
        /// Recursive move of files and folders from source to destination.
        /// </summary>
        public static async Task MoveContentsRecursiveAsync(this IStorageFolder source, IStorageFolder dest, CancellationToken token = default(CancellationToken))
        {
            await MoveContentsShallowAsync(source, dest, token);

            var subfolders = await source.GetFoldersAsync();
            foreach (var storageFolder in subfolders)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                await storageFolder.MoveContentsRecursiveAsync(await dest.GetFolderAsync(storageFolder.Name), token);
                await storageFolder.DeleteAsync();
            }
        }

        /// <summary>
        /// Shallow move of files and folders from source to destination.
        /// </summary>
        public static async Task MoveContentsShallowAsync(this IStorageFolder source, IStorageFolder destination, CancellationToken token = default(CancellationToken))
        {
            await source.MoveFilesAsync(destination, token);

            var items = await source.GetFoldersAsync();

            foreach (var storageFolder in items)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                await destination.CreateFolderAsync(storageFolder.Name, CreationCollisionOption.ReplaceExisting);
                await storageFolder.DeleteAsync();
            }
        }

        /// <summary>
        /// Move files from source into destination folder.
        /// </summary>
        private static async Task MoveFilesAsync(this IStorageFolder source, IStorageFolder destination, CancellationToken token = default(CancellationToken))
        {
            var items = await source.GetFilesAsync();

            foreach (var storageFile in items)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                await storageFile.MoveAsync(destination, storageFile.Name, NameCollisionOption.ReplaceExisting);
            }
        }
    }
}