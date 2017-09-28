using kmd.Core.Explorer.Hotkeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace kdm.Core.Explorer.Commands.Abstractions
{
    public class CommandBindings
    {
        private IEnumerable<CommandInfo> _underlyingCommands;

        public ICommand this[string name]
        {
            get
            {
                return GetCommandByName(name);
            }
        }

        public CommandBindings(IEnumerable<CommandInfo> commands)
        {
            _underlyingCommands = commands ?? throw new ArgumentNullException(nameof(commands));
        }

        public ICommand GetCommandByName(string name)
        {
            return _underlyingCommands.FirstOrDefault(x => x.Name == name)?.Command;
        }

        public ICommand GetCommandByHotkey(Hotkey hotkey)
        {
            return _underlyingCommands.FirstOrDefault(x => x.Hotkey == hotkey)?.Command;
        }
    }
}