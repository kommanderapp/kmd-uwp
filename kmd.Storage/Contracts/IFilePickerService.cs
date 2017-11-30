using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Contracts
{
    public interface IFilePickerService
    {
        Task<IStorageFile> PickSingleAsync();
    }
}
