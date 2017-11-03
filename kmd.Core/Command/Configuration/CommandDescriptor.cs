using kmd.Core.Hotkeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kmd.Core.Command.Configuration
{
    public class CommandDescriptor
    {
        public string UniqueName { get; protected set; }
        public string Description { get; protected set; }
        public Hotkey PreferredHotkey { get; set; }
        public Hotkey DefaultHotkey { get; protected set; }
        public bool HasHotkey => DefaultHotkey != null;
    }
}
