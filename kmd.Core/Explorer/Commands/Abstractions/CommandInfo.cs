using kmd.Core.Hotkeys;
using System;
using System.Windows.Input;

namespace kmd.Core.Explorer.Commands.Abstractions
{
    public class CommandInfo
    {
        public CommandInfo(string name, ICommand command) : this(name, command, null)
        {
        }

        public CommandInfo(string name, ICommand command, Hotkey hotkey)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
            Command = command ?? throw new ArgumentNullException(nameof(command));
            Hotkey = hotkey;
        }

        public ICommand Command { get; }
        public Hotkey Hotkey { get; }
        public string Name { get; }
    }
}