using System;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace kmd.Core.Explorer.Contracts
{
    public interface IExplorerItem
    {
        IStorageFile AsFile { get; }
        IStorageFolder AsFolder { get; }
        IStorageItemProperties Props { get; }
        IStorageItem2 StorageItem { get; }
        ImageSource Icon { get; }
        bool IsPhysical { get; }
        bool IsFile { get; }
        bool IsFolder { get; }
        DateTimeOffset DateCreated { get; }
        FileAttributes Attributes { get; }
        string ContentType { get; }
        string DisplayName { get; }
        string DisplayType { get; }
        string FileType { get; }
        string Name { get; }
        string Path { get; }
    }
}