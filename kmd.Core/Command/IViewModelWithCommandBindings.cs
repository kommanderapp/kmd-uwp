using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Command
{
    public interface IViewModelWithCommandBindings
    {
        CommandBindings CommandBindings { get; }
    }
}