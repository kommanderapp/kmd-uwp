using System;
using System.Windows.Input;

namespace kmd.Core.Command
{
    public abstract class CommandBase : ICommand
    {
#pragma warning disable CS0067
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS0067

        public bool CanExecute(object parameter) => OnCanExecute(parameter);

        public void Execute(object parameter) => OnExecute(parameter);
        protected abstract bool OnCanExecute(object parameter);

        protected abstract void OnExecute(object parameter);
    }
}