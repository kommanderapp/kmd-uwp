using kmd.Core.Hotkeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace kmd.Core.Explorer.Commands.Abstractions
{
    public class CommandBindings
    {
        public CommandBindings(IEnumerable<CommandInfo> commands)
        {
            _underlyingCommands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public ICommand this[string name]
        {
            get
            {
                return GetCommandByName(name);
            }
        }

        public ICommand GetCommandByHotkey(Hotkey hotkey)
        {
            return _underlyingCommands.FirstOrDefault(x => x.Hotkey == hotkey)?.Command;
        }

        public ICommand GetCommandByName(string name)
        {
            return _underlyingCommands.FirstOrDefault(x => x.Name == name)?.Command;
        }

        private IEnumerable<CommandInfo> _underlyingCommands;
    }
}