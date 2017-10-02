using System;

namespace kmd.Core.Hotkeys
{
    public class HotkeyEventArg : EventArgs
    {
        public HotkeyEventArg(Hotkey hotkey)
        {
            Hotkey = hotkey ?? throw new ArgumentNullException(nameof(hotkey));
        }

        public bool Handled { get; set; }
        public Hotkey Hotkey { get; }
    }
}