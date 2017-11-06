using kmd.Core.Hotkeys;

namespace kmd.Core.Command.Configuration
{
    public abstract class CommandDescriptor
    {
        public string UniqueName { get; protected set; }
        public string Description { get; protected set; }
        public Hotkey PreferredHotkey { get; set; }
        public Hotkey DefaultHotkey { get; protected set; }
        public bool HasHotkey => DefaultHotkey != null;
    }
}
