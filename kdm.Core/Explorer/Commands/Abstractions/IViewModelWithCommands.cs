using kdm.Core.Explorer.Commands;
using kdm.Core.Explorer.Commands.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kdm.Core.Commands.Abstractions
{
    public interface IViewModelWithCommands
    {
        CommandBindings CommandBindings { get; }
    }
}