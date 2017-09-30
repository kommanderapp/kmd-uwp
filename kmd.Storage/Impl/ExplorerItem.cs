using kmd.Storage.Contracts;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace kmd.Storage.Impl
{
    public class ExplorerItem : IExplorerItem
    {
        public static async Task<ExplorerItem> CreateAsync(IStorageItem2 storageItem)
        {
            var model = new ExplorerItem(storageItem);
            await model.InitializeAsync();
            return model;
        }

        private ExplorerItem(IStorageItem2 storageItem)
        {
            StorageItem = storageItem ?? throw new ArgumentNullException(nameof(storageItem));
        }

        protected async Task InitializeAsync()
        {
            Icon = await GetIconAsync();
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

        public IStorageFile AsFile { get => StorageItem as IStorageFile; }
        public IStorageFolder AsFolder { get => StorageItem as IStorageFolder; }
        public IStorageItemProperties Props { get => StorageItem as IStorageItemProperties; }
        public IStorageItem2 StorageItem { get; }
        public ImageSource Icon { get; internal set; }
        public bool IsFile => StorageItem.IsOfType(StorageItemTypes.File);
        public bool IsFolder => StorageItem.IsOfType(StorageItemTypes.Folder);
        public DateTimeOffset DateCreated => StorageItem.DateCreated;
        public FileAttributes Attributes { get => StorageItem.Attributes; }
        public string ContentType { get => IsFolder ? null : AsFile?.ContentType; }
        public string DisplayName { get => Props?.DisplayName; }
        public string DisplayType { get => Props?.DisplayType; }
        public string FileType { get => IsFolder ? null : AsFile?.FileType; }
        public string Name { get => StorageItem.Name; }
        public string Path { get => StorageItem.Path; }

        public bool IsPhysical => true;
    }
}