using kmd.Storage.Contracts;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace kmd.Core.Explorer
{
    public class UpperFolderLinkModel : IExplorerItem
    {
        public static async Task<UpperFolderLinkModel> CreateAsync(IStorageFolder innerFolder)
        {
            var upperModel = new UpperFolderLinkModel(innerFolder);
            return await Task.FromResult(upperModel);
        }

        private IStorageFolder _innerFolder;

        public UpperFolderLinkModel(IStorageFolder innerFolder)
        {
            _innerFolder = innerFolder ?? throw new ArgumentNullException(nameof(innerFolder));
        }

        public IStorageFile AsFile => null;

        public IStorageFolder AsFolder => _innerFolder;

        public IStorageItemProperties Props => StorageItem as IStorageItemProperties;

        public IStorageItem2 StorageItem => _innerFolder as IStorageItem2;

        public ImageSource Icon => null;

        public bool IsFile => false;

        public bool IsFolder => true;

        public DateTimeOffset DateCreated => _innerFolder.DateCreated;

        public FileAttributes Attributes => _innerFolder.Attributes;

        public string ContentType => null;

        public string DisplayName => Props?.DisplayName;

        public string DisplayType => Props?.DisplayType;

        public string FileType => null;

        public string Name => "...";

        public string Path => _innerFolder.Path;
        public bool IsPhysical => false;
    }
}