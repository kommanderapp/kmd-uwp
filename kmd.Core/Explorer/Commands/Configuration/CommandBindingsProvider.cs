using kmd.Core.Command;
using kmd.Core.Explorer.Contracts;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace kmd.Core.Explorer.Commands.Configuration
{
    public static class CommandBindingsProvider
    {
        public static CommandBindings GetBindings(IExplorerViewModel explorerViewModel)
        {
            if (explorerViewModel == null) throw new Exception(nameof(explorerViewModel));

            var commandInfos = new List<CommandInfo>();
            var serviceLocator = ServiceLocator.Current;

            var explorerCommandDescriptors = GetExplorerCommandDescriptors();

            foreach (var commandDescriptor in explorerCommandDescriptors)
            {
                var command = serviceLocator.GetInstance(commandDescriptor.Type) as ICommand;
                if (command == null) throw new Exception($"No instance registered for {commandDescriptor.Type.FullName} command.");

                var commandName = commandDescriptor.Attribute.Name ?? commandDescriptor.Type.Name;
                var commandHotkey = commandDescriptor.Attribute.Hotkey ?? null; // TODO get hotkey from settings
                var commandInfo = new CommandInfo(commandName, command, commandHotkey);
                commandInfos.Add(commandInfo);
            }

            var bindings = new CommandBindings(commandInfos);
            return bindings;
        }

        private static IEnumerable<ExplorerCommandDescriptor> GetExplorerCommandDescriptors()
        {
            var assembly = typeof(CommandBase).GetTypeInfo().Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetTypeInfo().GetCustomAttributes(typeof(ExplorerCommandAttribute), true).Count() > 0)
                {
                    if (type.GetTypeInfo()
                        .GetCustomAttributes(typeof(ExplorerCommandAttribute), true)
                        .FirstOrDefault() is ExplorerCommandAttribute commandAttr)
                    {
                        yield return new ExplorerCommandDescriptor(type, commandAttr);
                    }
                }
            }
        }

        private class ExplorerCommandDescriptor
        {
            public ExplorerCommandDescriptor(Type type, ExplorerCommandAttribute attribute)
            {
                Type = type ?? throw new ArgumentNullException(nameof(type));
                Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
            }

            public ExplorerCommandAttribute Attribute { get; }
            public Type Type { get; }
        }
    }
}