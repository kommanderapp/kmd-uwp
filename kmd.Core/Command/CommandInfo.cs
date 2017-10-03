using kmd.Core.Hotkeys;
using System;
using System.Windows.Input;

namespace kmd.Core.Command
{
    public class CommandInfo
    {
        public CommandInfo(string name, ICommand command) : this(name, command, null, null)
        {
        }

        public CommandInfo(string name, ICommand command, Hotkey hotkey) : this(name, command, hotkey, null)
        {
        }

        public CommandInfo(string name, ICommand command, Hotkey hotkey, string shortcutText)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));

            Name = name;
            Hotkey = hotkey;
            Command = command ?? throw new ArgumentNullException(nameof(command));
            ShortcutText = shortcutText;
        }

        public ICommand Command { get; }
        public Hotkey Hotkey { get; }
        public string Name { get; }
        public string ShortcutText { get; }
    }
}