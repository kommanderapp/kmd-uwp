using System;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace kmd.Core.Explorer.Contracts
{
    public interface IExplorerItem
    {
        IStorageFile AsFile { get; }
        IStorageFolder AsFolder { get; }
        FileAttributes Attributes { get; }
        string ContentType { get; }
        DateTimeOffset DateCreated { get; }
        string DisplayName { get; }
        string DisplayType { get; }
        string FileType { get; }
        ImageSource Icon { get; }
        bool IsFile { get; }
        bool IsFolder { get; }
        bool IsPhysical { get; }
        string Name { get; }
        string Path { get; }
        IStorageItemProperties Props { get; }
        IStorageItem2 StorageItem { get; }
    }
}