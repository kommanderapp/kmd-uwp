using kmd.Core.Explorer.Contracts;
using System;
using System.Windows.Input;

namespace kmd.Core.Explorer.Commands.Abstractions
{
    public abstract class ExplorerCommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public IExplorerViewModel ViewModel
        {
            get
            {
                if (_model == null)
                {
                    throw new Exception("Model isn't initialized for ExplorerCommand that want to access it.");
                }
                return _model;
            }
            set
            {
                _model = value;
            }
        }

        public bool CanExecute(object parameter) => OnCanExecute(parameter);

        public void Execute(object parameter)
        {
            if (CanExecute(parameter))
            {
                OnExecute(parameter);
            }
        }

        protected abstract bool OnCanExecute(object parameter);

        protected abstract void OnExecute(object parameter);

        private IExplorerViewModel _model;
    }
}