using CommonServiceLocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Command.Configuration
{
    public static class CommandDescriptorProvider
    {
        private static IEnumerable<ICommandDescriptorFactory> _descriptorFactories;

        static CommandDescriptorProvider()
        {
            var factories = typeof(CommandDescriptorProvider)
                .Assembly.DefinedTypes.Where(x => typeof(ICommandDescriptorFactory).IsAssignableFrom(x) && !x.IsInterface);

            var descriptorFactories = new List<ICommandDescriptorFactory>();
            foreach (var factory in factories)
            {
                descriptorFactories.Add(Activator.CreateInstance(factory, false) as ICommandDescriptorFactory);
            }
            _descriptorFactories = descriptorFactories;
        }

        public static IEnumerable<CommandDescriptor> GetCommandDescriptors()
        {
            var descriptors = new List<CommandDescriptor>();
            foreach (var factory in _descriptorFactories)
            {
                descriptors.AddRange(factory.CreateCommandDescriptors());
            }
            return descriptors;
        }
    }
}
