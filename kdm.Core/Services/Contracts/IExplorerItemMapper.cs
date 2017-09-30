using kmd.Core.Explorer.Contracts;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace kdm.Core.Services.Contracts
{
    public interface IExplorerItemMapper
    {
        Task<ObservableCollection<IExplorerItem>> MapAsync(IEnumerable<IStorageItem2> items);
    }
}