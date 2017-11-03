using kmd.Core.Command.Configuration;
using kmd.Core.Helpers;
using kmd.Core.Hotkeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
