using kmd.Core.Explorer.Contracts;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace kmd.Core.Explorer.Models
{
    public class ExplorerItem : IExplorerItem
    {
        public IStorageFile AsFile { get => StorageItem as IStorageFile; }

        public IStorageFolder AsFolder { get => StorageItem as IStorageFolder; }

        public FileAttributes Attributes { get => StorageItem.Attributes; }

        public string ContentType { get => IsFolder ? null : AsFile?.ContentType; }

        public DateTimeOffset DateCreated => StorageItem.DateCreated;

        public string DisplayName { get => Props?.DisplayName; }

        public string DisplayType { get => Props?.DisplayType; }

        public string FileType { get => IsFolder ? null : AsFile?.FileType; }

        public ImageSource Icon { get; internal set; }

        public bool IsFile => StorageItem.IsOfType(StorageItemTypes.File);

        public bool IsFolder => StorageItem.IsOfType(StorageItemTypes.Folder);

        public bool IsPhysical => true;

        public string Name { get => StorageItem.Name; }

        public string Path { get => StorageItem.Path; }

        public IStorageItemProperties Props { get => StorageItem as IStorageItemProperties; }

        public IStorageItem2 StorageItem { get; }

        public static async Task<ExplorerItem> CreateAsync(IStorageItem2 storageItem)
        {
            var model = new ExplorerItem(storageItem);
            await model.InitializeAsync();
            return model;
        }

        protected async Task<BitmapImage> GetIconAsync()
        {
            BitmapImage iconImage = null;
            var thumb = await Props?.GetThumbnailAsync(ThumbnailMode.SingleItem, 16, ThumbnailOptions.UseCurrentScale)
;
            if (thumb != null)
            {
                iconImage = new BitmapImage();
                iconImage.SetSource(thumb.CloneStream());
            }

            return iconImage;
        }

        protected async Task InitializeAsync()
        {
            Icon = await GetIconAsync();
        }

        private ExplorerItem(IStorageItem2 storageItem)
        {
            StorageItem = storageItem ?? throw new ArgumentNullException(nameof(storageItem));
        }
    }
}