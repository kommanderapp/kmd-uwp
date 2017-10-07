using kmd.Helpers;
using System;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace kmd.Core.Hotkeys
{
    public static class KeyEventsAgregator
    {
        static KeyEventsAgregator()
        {
            var window = Window.Current.CoreWindow;
            if (window == null) throw new InvalidOperationException("KeyEventsAgregator accessed not in a time, when window is initialized.");

            window.KeyUp += KeyUpHandler;
            window.KeyDown += KeyDownHandler;
            window.CharacterReceived += CharacterReceivedHandler;
        }

        public static event EventHandler<CharReceivedEventArgs> CharacterReceived;

        public static event EventHandler<HotkeyEventArg> HotKey;

        private static bool _isCtrlKeyPressed = false;

        private static void CharacterReceivedHandler(CoreWindow sender, CharacterReceivedEventArgs e)
        {
            var uniChar = Unicode.ToString(e.KeyCode);
            var args = new CharReceivedEventArgs(uniChar);
            CharacterReceived?.Invoke(sender, args);
            e.Handled = args.Handled;
        }

        private static void KeyDownHandler(CoreWindow sender, KeyEventArgs e)
        {
            if (e.VirtualKey == VirtualKey.Control)
            {
                _isCtrlKeyPressed = true;
                return;
            }

            ModifierKeys modifierKey = ModifierKeys.None;
            if (_isCtrlKeyPressed) modifierKey = ModifierKeys.Control;

            var hotkey = Hotkey.For(modifierKey, e.VirtualKey);
            var args = new HotkeyEventArg(hotkey);
            HotKey?.Invoke(sender, args);
            e.Handled = args.Handled;
        }

        private static void KeyUpHandler(CoreWindow sender, KeyEventArgs e)
        {
            if (e.VirtualKey == VirtualKey.Control) _isCtrlKeyPressed = false;
        }
    }
}