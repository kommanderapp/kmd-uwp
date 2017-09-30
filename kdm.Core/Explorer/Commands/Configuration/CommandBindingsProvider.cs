using kdm.Core.Explorer.Commands.Abstractions;
using kmd.Core.Explorer.Contracts;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace kdm.Core.Explorer.Commands.Configuration
{
    public class CommandBindingsProvider : ICommandBindingsProvider
    {
        public CommandBindings GetBindings(IExplorerViewModel explorerViewModel)
        {
            if (explorerViewModel == null) throw new Exception(nameof(explorerViewModel));

            var commandInfos = new List<CommandInfo>();
            var serviceLocator = ServiceLocator.Current;

            var explorerCommandDescriptors = GetExplorerCommandDescriptors();

            foreach (var commandDescriptor in explorerCommandDescriptors)
            {
                var command = serviceLocator.GetInstance(commandDescriptor.Type) as ICommand;
                if (command == null) throw new Exception($"No instance registered for {commandDescriptor.Type.FullName} command.");

                if (command is ExplorerCommandBase)
                {
                    (command as ExplorerCommandBase).ViewModel = explorerViewModel;
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
            var assembly = typeof(ExplorerCommandBase).GetTypeInfo().Assembly;
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetTypeInfo().GetCustomAttributes(typeof(ExplorerCommandAttribute), true).Count() > 0)
                {
                    if (type.GetTypeInfo()
                        .GetCustomAttributes(typeof(ExplorerCommandAttribute), true)
                        .FirstOrDefault() is ExplorerCommandAttribute commandAttr)
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