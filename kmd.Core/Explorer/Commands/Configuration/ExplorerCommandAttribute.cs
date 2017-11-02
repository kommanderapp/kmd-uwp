using kmd.Core.Hotkeys;
using System;
using Windows.System;

namespace kmd.Core.Explorer.Commands.Configuration
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExplorerCommandAttribute : Attribute
    {
        public ExplorerCommandAttribute() : this(null, null, ModifierKeys.None, VirtualKey.None)
        {

        }

        public ExplorerCommandAttribute(string name, string shortcutText, ModifierKeys modifierKey, VirtualKey key)
        {
            Name = name;
            ShortcutText = shortcutText;
            if (modifierKey != ModifierKeys.None || key != VirtualKey.None)
            {
                DefaultHotkey = Hotkey.For(modifierKey, key);
            }
        }

        public Hotkey DefaultHotkey { get; }
        public string Name { get; }
        public string ShortcutText { get; set; }
    }
}
