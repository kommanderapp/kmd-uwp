using System;

namespace kmd.Core.Explorer.Hotkeys
{
    public class HotkeyEventArg : EventArgs
    {
        public Hotkey Hotkey { get; }
        public bool Handled { get; set; }

        public HotkeyEventArg(Hotkey hotkey)
        {
            Hotkey = hotkey ?? throw new ArgumentNullException(nameof(hotkey));
        }
    }
}