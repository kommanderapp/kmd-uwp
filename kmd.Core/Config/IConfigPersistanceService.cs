using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Config
{
    public interface IConfigPersistanceService<T> where T : new()
    {
        Task<T> LoadAsync();

        Task SaveAsync(T settings);
    }
}