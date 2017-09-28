using kmd.Storage.Contracts;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace kmd.Storage.Impl
{
    public class FolderPickerService : IFolderPickerService
    {
        public async Task<IStorageFolder> PickSingleAsync()
        {
            FolderPicker picker = new FolderPicker() { SuggestedStartLocation = PickerLocationId.ComputerFolder };
            picker.FileTypeFilter.Add("*");
            picker.ViewMode = PickerViewMode.List;
            var folder = await picker.PickSingleFolderAsync();
            return folder;
        }
    }
}