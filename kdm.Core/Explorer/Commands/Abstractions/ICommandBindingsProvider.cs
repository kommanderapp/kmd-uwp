using kmd.Core.Explorer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdm.Core.Explorer.Commands.Abstractions
{
    public interface ICommandBindingsProvider
    {
        CommandBindings GetBindings(IExplorerModel explorerModel);
    }
}