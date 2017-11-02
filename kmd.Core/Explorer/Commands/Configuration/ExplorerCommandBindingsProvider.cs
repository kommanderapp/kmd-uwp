using kmd.Core.Command;
using kmd.Core.Explorer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using kmd.Core.Hotkeys;
using System.Threading.Tasks;

namespace kmd.Core.Explorer.Commands.Configuration
{
    public class ExplorerCommandBindingsProvider : IExplorerCommandBindingsProvider
    {
        private static IEnumerable<ExplorerCommandDescriptor> _explorerCommandDescriptors;
        public static IEnumerable<ExplorerCommandDescriptor> ExplorerCommandDescriptors
        {
            get
            {
                if (_explorerCommandDescriptors == null)
                {
                    _explorerCommandDescriptors = GetExplorerCommandDescriptors();
                }
                return _explorerCommandDescriptors;
            }            
        }

        public static Func<Type, object> Resolve { private get; set; }

        private static IEnumerable<ExplorerCommandDescriptor> GetExplorerCommandDescriptors()
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
                        //TODO find a better solution
                        var prefferedHotkey = HotkeyPersistenceService.GetPrefferedHotkey(commandAttr);
                        descriptors.Add(new ExplorerCommandDescriptor(type, commandAttr, prefferedHotkey));
                    }
                }
            }
            return descriptors;
        }

        public async static Task RefreshCommandBindingsAsync()
        {
            await Task.Run(() => _explorerCommandDescriptors = GetExplorerCommandDescriptors());            
        }

        public CommandBindings GetBindings(IExplorerViewModel explorerViewModel)
        {
            if (Resolve == null)
            {
                throw new InvalidOperationException("CommandBindngs Resolve must be set in application startup.");
            }

            if (explorerViewModel == null) throw new Exception(nameof(explorerViewModel));

            var commandBindings = new List<CommandBinding>();

            foreach (var commandDescriptor in ExplorerCommandDescriptors)
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
