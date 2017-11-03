using kmd.Core.Command;
using kmd.Core.Command.Configuration;
using kmd.Core.Hotkeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Explorer.Commands.Configuration
{
    public class ExplorerCommandDescriptorFactory : ICommandDescriptorFactory
    {
        public IEnumerable<CommandDescriptor> CreateCommandDescriptors()
        {
            var descriptors = new List<ExplorerCommandDescriptor>();
            var assembly = typeof(CommandBase).GetTypeInfo().Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetTypeInfo().GetCustomAttributes(typeof(ExplorerCommandAttribute), true).Any())
                {
                    if (type.GetTypeInfo()
                        .GetCustomAttributes(typeof(ExplorerCommandAttribute), true)
                        .FirstOrDefault() is ExplorerCommandAttribute commandAttr)
                    {
                        var descriptor = new ExplorerCommandDescriptor(type, commandAttr);
                        HotkeyPersistenceService.SetPrefferedHotkey(descriptor);
                        descriptors.Add(descriptor);
                    }
                }
            }
            return descriptors;
        }
    }
}
