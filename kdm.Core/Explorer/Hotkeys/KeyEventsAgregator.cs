using System;
using Windows.System;
using Windows.UI.Xaml.Input;

namespace kmd.Core.Explorer.Hotkeys
{
    public class KeyEventsAgregator
    {
        protected bool _isCtrlKeyPressed = false;

        public event EventHandler<HotkeyEventArg> HotKey;

        public void KeyDownHandler(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Control)
            {
                _isCtrlKeyPressed = true;
                return;
            }

            ModifierKeys modifierKey = ModifierKeys.None;
            if (_isCtrlKeyPressed) modifierKey = ModifierKeys.Control;

            var hotkey = Hotkey.For(modifierKey, e.Key);
            var hotkeyEvent = new HotkeyEventArg(hotkey);
            HotKey?.Invoke(this, hotkeyEvent);
            e.Handled = hotkeyEvent.Handled;
        }

        public void KeyUpHandler(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Control) _isCtrlKeyPressed = false;
        }
    }
}