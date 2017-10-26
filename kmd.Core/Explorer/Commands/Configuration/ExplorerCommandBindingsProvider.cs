using kmd.Core.Command;
using kmd.Core.Explorer.Contracts;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using kmd.Core.Hotkeys;

namespace kmd.Core.Explorer.Commands.Configuration
{
    public class ExplorerCommandBindingsProvider : IExplorerCommandBindingsProvider
    {
        public static Func<Type, object> Resolve { private get; set; }

        public static IEnumerable<ExplorerCommandDescriptor> GetExplorerCommandDescriptors()
        {
            var assembly = typeof(CommandBase).GetTypeInfo().Assembly;
            foreach (var type in assembly.GetTypes())
            {
                if (type.GetTypeInfo().GetCustomAttributes(typeof(ExplorerCommandAttribute), true).Any())
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

        public CommandBindings GetBindings(IExplorerViewModel explorerViewModel)
        {
            if (Resolve == null)
            {
                throw new InvalidOperationException("CommandBindngs Resolve must be set in application startup.");
            }

            if (explorerViewModel == null) throw new Exception(nameof(explorerViewModel));

            var commandBindings = new List<CommandBinding>();

            var explorerCommandDescriptors = GetExplorerCommandDescriptors();

            foreach (var commandDescriptor in explorerCommandDescriptors)
            {
                var command = Resolve(commandDescriptor.Type) as ICommand;
                if (command == null)
                {
                    throw new Exception($"No instance resoled for {commandDescriptor.Type.FullName} command.");
                }

                var commandName = commandDescriptor.Attribute.Name ?? commandDescriptor.Type.Name;
                var commandPreferredHotkey = commandDescriptor.PreferredHotkey ?? null;
                var commandInfo = new CommandBinding(commandName, command, commandPreferredHotkey);
                commandBindings.Add(commandInfo);
            }

            var bindings = new CommandBindings(commandBindings);
            return bindings;
        }
    }
}