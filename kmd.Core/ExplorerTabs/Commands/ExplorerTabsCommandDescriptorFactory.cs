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
            var removeTabDescriptor = new ExplorerTabsCommandDescriptor("RemoveTab", "ExplorerTabsCommands_RemoveTab", Hotkey.For(ModifierKeys.Control, Windows.System.VirtualKey.W));
            var copyToOtherExplorerDescriptor = new ExplorerTabsCommandDescriptor("CopyToOtherExplorer", "ExplorerTabsCommands_CopyToOtherExplorer", Hotkey.For(ModifierKeys.None, Windows.System.VirtualKey.F5));
            var moveToOtherExplorerDescriptor = new ExplorerTabsCommandDescriptor("MoveToOtherExplorer", "ExplorerTabsCommands_MoveToOtherExplorer", Hotkey.For(ModifierKeys.None, Windows.System.VirtualKey.F6));

            HotkeyPersistenceService.SetPrefferedHotkey(addTabDescriptor);
            HotkeyPersistenceService.SetPrefferedHotkey(removeTabDescriptor);
            HotkeyPersistenceService.SetPrefferedHotkey(copyToOtherExplorerDescriptor);
            HotkeyPersistenceService.SetPrefferedHotkey(moveToOtherExplorerDescriptor);

            descriptors.Add(addTabDescriptor);
            descriptors.Add(removeTabDescriptor);
            descriptors.Add(copyToOtherExplorerDescriptor);
            descriptors.Add(moveToOtherExplorerDescriptor);

            return descriptors;
        }
    }
}
