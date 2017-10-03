using kmd.Core.Hotkeys;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands.Configuration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExplorerCommandAttribute : Attribute
    {
        public ExplorerCommandAttribute(string name = null, string shortcutText = null, ModifierKeys modifierKey = ModifierKeys.None, VirtualKey key = VirtualKey.None)
        {
            Name = name;
            ShortcutText = shortcutText;
            if (modifierKey != ModifierKeys.None || key != VirtualKey.None)
            {
                Hotkey = Hotkey.For(modifierKey, key);
            }
        }

        public Hotkey Hotkey { get; }
        public string Name { get; }
        public string ShortcutText { get; set; }
    }
}