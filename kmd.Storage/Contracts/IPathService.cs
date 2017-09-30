using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;

namespace kmd.Storage.Contracts
{
    public interface IPathService
    {
        Task<IEnumerable<IStorageFolder>> ExpandParents(IStorageFolder folder);

        Task<IStorageFolder> GetRootAsync(IStorageFolder folder);

        Task<IStorageFolder> GoToAsync(IStorageFolder root, string path);
    }
}