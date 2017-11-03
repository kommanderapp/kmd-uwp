using kmd.Core.Command;
using kmd.Core.Explorer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using kmd.Core.Hotkeys;
using System.Threading.Tasks;
using kmd.Core.Command.Configuration;

namespace kmd.Core.Explorer.Commands.Configuration
{
    public class ExplorerCommandBindingsProvider : IExplorerCommandBindingsProvider
    {
        public static Func<Type, object> Resolve { private get; set; }

        public CommandBindings GetBindings(IExplorerViewModel explorerViewModel)
        {
            if (Resolve == null)
            {
                throw new InvalidOperationException("CommandBindngs Resolve must be set in application startup.");
            }

            if (explorerViewModel == null) throw new Exception(nameof(explorerViewModel));

            var commandBindings = new List<CommandBinding>();
            var descriptors = CommandDescriptorProvider.GetCommandDescriptors()
                .Where(x => x is ExplorerCommandDescriptor).Cast<ExplorerCommandDescriptor>();
            foreach (var commandDescriptor in descriptors)
            {
                var command = Resolve(commandDescriptor.CommandType) as ICommand;
                if (command == null)
                {
                    throw new Exception($"No instance resoled for {commandDescriptor.CommandType.FullName} command.");
                }

                var commandName = commandDescriptor.Attribute.UniqueName ?? commandDescriptor.CommandType.Name;
                var commandPreferredHotkey = commandDescriptor.PreferredHotkey ?? null;
                var commandInfo = new CommandBinding(commandName, command, commandPreferredHotkey);
                commandBindings.Add(commandInfo);
            }

            var bindings = new CommandBindings(commandBindings);
            return bindings;
        }
    }
}
