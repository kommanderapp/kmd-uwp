using kdm.Core.Explorer.Commands.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using kmd.Core.Explorer.Contracts;
using Microsoft.Practices.ServiceLocation;
using System.Reflection;
using System.Windows.Input;
using kmd.Core.Explorer.Hotkeys;

namespace kdm.Core.Explorer.Commands
{
    public class CommandBindingsProvider : ICommandBindingsProvider
    {
        public CommandBindings GetBindings(IExplorerModel explorerModel)
        {
            if (explorerModel == null) throw new Exception(nameof(explorerModel));

            var commandInfos = new List<CommandInfo>();
            var serviceLocator = ServiceLocator.Current;

            var explorerCommandDescriptors = GetExplorerCommandDescriptors();

            foreach (var commandDescriptor in explorerCommandDescriptors)
            {
                var command = serviceLocator.GetInstance(commandDescriptor.Type) as ICommand;
                if (command == null) throw new Exception($"No instance registered for {commandDescriptor.Type.FullName} command.");

                if (command is ExplorerCommand)
                {
                    (command as ExplorerCommand).Model = explorerModel;
                }

                var commandName = commandDescriptor.Attribute.Name ?? commandDescriptor.Type.Name;
                var commandHotkey = commandDescriptor.Attribute.Hotkey ?? null; // TODO get hotkey from settings
                var commandInfo = new CommandInfo(commandName, command, commandHotkey);
                commandInfos.Add(commandInfo);
            }

            var bindings = new CommandBindings(commandInfos);
            return bindings;
        }

        private static IEnumerable<CommandDescriptor> GetExplorerCommandDescriptors()
        {
            var assembly = typeof(ExplorerCommand).GetTypeInfo().Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetTypeInfo().GetCustomAttributes(typeof(ExplorerCommandAttribute), true).Count() > 0)
                {
                    var commandAttr = type.GetTypeInfo()
                        .GetCustomAttributes(typeof(ExplorerCommandAttribute), true)
                        .FirstOrDefault() as ExplorerCommandAttribute;
                    if (commandAttr != null)
                    {
                        yield return new CommandDescriptor(type, commandAttr);
                    }
                }
            }
        }

        private class CommandDescriptor
        {
            public CommandDescriptor(Type type, ExplorerCommandAttribute attribute)
            {
                Type = type ?? throw new ArgumentNullException(nameof(type));
                Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
            }

            public Type Type { get; }
            public ExplorerCommandAttribute Attribute { get; }
        }
    }
}