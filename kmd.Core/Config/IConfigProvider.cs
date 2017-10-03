using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Config
{
    public interface IConfigProvider<T>
    {
        T Create();

        T CreateDefaults();

        void Save(T settings);
    }
}