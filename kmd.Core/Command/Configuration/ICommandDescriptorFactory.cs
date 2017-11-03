using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Command.Configuration
{
    public interface ICommandDescriptorFactory
    {
        IEnumerable<CommandDescriptor> CreateCommandDescriptors();
    }
}
