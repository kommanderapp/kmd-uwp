using kmd.Core.Command.Configuration;
using kmd.Core.Hotkeys;
using System.Collections.Generic;

namespace kmd.Core.ExplorerTabs.Commands
{
    public class ExplorerTabsCommandDescriptorFactory : ICommandDescriptorFactory
    {
        public IEnumerable<CommandDescriptor> CreateCommandDescriptors()
        {
            var descriptors = new List<ExplorerTabsCommandDescriptor>();
            var addTabDescriptor = new ExplorerTabsCommandDescriptor("AddTab", "ExplorerTabsCommands_AddTab", Hotkey.For(ModifierKeys.Control, Windows.System.VirtualKey.T));
            HotkeyPersistenceService.SetPrefferedHotkey(addTabDescriptor);
            var removeTabDescriptor = new ExplorerTabsCommandDescriptor("RemoveTab", "ExplorerTabsCommands_RemoveTab", Hotkey.For(ModifierKeys.Control, Windows.System.VirtualKey.W));
            HotkeyPersistenceService.SetPrefferedHotkey(removeTabDescriptor);
            descriptors.Add(addTabDescriptor);
            descriptors.Add(removeTabDescriptor);
            return descriptors;
        }
    }
}
