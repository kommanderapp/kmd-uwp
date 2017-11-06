using System.Collections.Generic;

namespace kmd.Core.Command.Configuration
{
    public interface ICommandDescriptorFactory
    {
        IEnumerable<CommandDescriptor> CreateCommandDescriptors();
    }
}
