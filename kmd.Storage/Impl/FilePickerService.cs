using kmd.Storage.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace kmd.Storage.Impl
{
    public class FilePickerService : IFilePickerService
    {
        public async Task<IStorageFile> PickSingleAsync()
        {
            FileOpenPicker picker = new FileOpenPicker() { SuggestedStartLocation = PickerLocationId.ComputerFolder };
            picker.FileTypeFilter.Add("*");
            picker.ViewMode = PickerViewMode.List;
            var file = await picker.PickSingleFileAsync();
            return file;
        }
    }
}
