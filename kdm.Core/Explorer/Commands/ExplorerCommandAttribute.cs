using kmd.Core.Explorer.Hotkeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace kdm.Core.Explorer.Commands
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ExplorerCommandAttribute : Attribute
    {
        public string Name { get; }
        public Hotkey Hotkey { get; }

        public ExplorerCommandAttribute(string name = null, ModifierKeys modifierKey = ModifierKeys.None, VirtualKey key = VirtualKey.None)
        {
            Name = name;
            if (modifierKey != ModifierKeys.None || key != VirtualKey.None)
            {
                Hotkey = Hotkey.For(modifierKey, key);
            }
        }
    }
}