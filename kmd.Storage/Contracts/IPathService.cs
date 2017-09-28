using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Contracts
{
    public interface IPathService
    {
        Task<IStorageFolder> GoToAsync(IStorageFolder root, string path);

        Task<IStorageFolder> GetRootAsync(IStorageFolder folder);

        Task<IEnumerable<IStorageFolder>> ExpandParents(IStorageFolder folder);
    }
}