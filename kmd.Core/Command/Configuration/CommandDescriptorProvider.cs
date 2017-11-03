using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Command.Configuration
{
    public static class CommandDescriptorProvider
    {
        private static List<ICommandDescriptorFactory> _commandDescriptorFactories = new List<ICommandDescriptorFactory>();

        public static void RegisterFactory(ICommandDescriptorFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));
            _commandDescriptorFactories.Add(factory);
        }

        public static IEnumerable<CommandDescriptor> GetCommandDescriptors()
        {
            var descriptors = new List<CommandDescriptor>();
            foreach (var factory in _commandDescriptorFactories)
            {
                descriptors.AddRange(factory.Create());
            }
            return descriptors;
        }
    }
}
