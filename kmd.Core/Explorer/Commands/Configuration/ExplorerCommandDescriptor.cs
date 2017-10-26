using System;
using kmd.Core.Hotkeys;

namespace kmd.Core.Explorer.Commands.Configuration
{
    public class ExplorerCommandDescriptor
    {
        public ExplorerCommandDescriptor(Type type, ExplorerCommandAttribute attribute)
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
        }

        public ExplorerCommandAttribute Attribute { get; }
        public Hotkey PreferredHotkey { get; set; }
        public Hotkey DefaultHotkey => Attribute.DefaultHotkey;
        public Type Type { get; }
    }
}