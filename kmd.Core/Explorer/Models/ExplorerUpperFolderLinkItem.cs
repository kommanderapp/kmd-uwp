using kmd.Core.Explorer.Contracts;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace kmd.Core.Explorer.Models
{
    public class ExplorerUpperFolderLinkItem : IExplorerItem
    {
        public ExplorerUpperFolderLinkItem(IStorageFolder innerFolder)
        {
            _innerFolder = innerFolder ?? throw new ArgumentNullException(nameof(innerFolder));
        }

        public IStorageFile AsFile => null;
        public IStorageFolder AsFolder => _innerFolder;
        public FileAttributes Attributes => _innerFolder.Attributes;
        public string ContentType => null;
        public DateTimeOffset DateCreated => _innerFolder.DateCreated;
        public string DisplayName => Props?.DisplayName;
        public string DisplayType => Props?.DisplayType;
        public string FileType => null;
        public ImageSource Icon { get; private set; }
        public bool IsFile => false;
        public bool IsFolder => true;
        public bool IsPhysical => false;
        public string Name => "...";
        public string Path => _innerFolder.Path;
        public IStorageItemProperties Props => StorageItem as IStorageItemProperties;
        public IStorageItem2 StorageItem => _innerFolder as IStorageItem2;

        public static async Task<ExplorerUpperFolderLinkItem> CreateAsync(IStorageFolder innerFolder)
        {
            var upperModel = new ExplorerUpperFolderLinkItem(innerFolder);
            await upperModel.InitializeAsync();
            return await Task.FromResult(upperModel);
        }

        protected async Task<BitmapImage> GetIconAsync()
        {
            BitmapImage iconImage = null;
            var thumb = await (_innerFolder as IStorageItemProperties)?.GetThumbnailAsync(ThumbnailMode.SingleItem, 96, ThumbnailOptions.UseCurrentScale)
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

        private IStorageFolder _innerFolder;
    }
}
