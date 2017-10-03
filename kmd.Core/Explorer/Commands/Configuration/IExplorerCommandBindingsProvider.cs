using kmd.Core.Command;
using kmd.Core.Explorer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Explorer.Commands.Configuration
{
    public interface IExplorerCommandBindingsProvider
    {
        CommandBindings GetBindings(IExplorerViewModel explorerViewModel);
    }
}