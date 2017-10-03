using kmd.Core.Explorer.Contracts;
using kmd.Core.Hotkeys;
using System;
using System.Windows.Input;

namespace kmd.Core.Command
{
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public virtual Hotkey Hotkey => null;
        public string ShortcutText { get; set; }

        public bool CanExecute(object parameter) => OnCanExecute(parameter);

        public async void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                OnExecute(parameter);
            }
        }

        protected abstract bool OnCanExecute(object parameter);

        protected abstract void OnExecute(object parameter);
    }
}