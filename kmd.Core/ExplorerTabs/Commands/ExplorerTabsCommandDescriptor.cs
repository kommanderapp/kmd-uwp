using kmd.Core.Command.Configuration;
using kmd.Core.Helpers;
using kmd.Core.Hotkeys;

namespace kmd.Core.ExplorerTabs.Commands
{
    public class ExplorerTabsCommandDescriptor : CommandDescriptor
    {
        public ExplorerTabsCommandDescriptor(string name, string description, Hotkey defaultHotkey)
        {
            this.UniqueName = name;
            this.Description = description.GetLocalized();
            this.DefaultHotkey = defaultHotkey;
        }
    }
}
