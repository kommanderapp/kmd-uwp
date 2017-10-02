using kmd.Core.Hotkeys;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands.Configuration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExplorerCommandAttribute : Attribute
    {
        public ExplorerCommandAttribute(string name = null, string description = null, ModifierKeys modifierKey = ModifierKeys.None, VirtualKey key = VirtualKey.None)
        {
            Name = name;
            Description = description;
            if (modifierKey != ModifierKeys.None || key != VirtualKey.None)
            {
                Hotkey = Hotkey.For(modifierKey, key);
            }
        }

        public string Description { get; set; }
        public Hotkey Hotkey { get; }
        public string Name { get; }
    }
}