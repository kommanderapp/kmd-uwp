using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Contracts
{
    public interface IFolderPickerService
    {
        Task<IStorageFolder> PickSingleAsync();
    }
}