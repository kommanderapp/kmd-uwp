using kmd.Core.Explorer.Contracts;
using System;
using System.Windows.Input;

namespace kdm.Core.Explorer.Commands.Abstractions
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

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);

        private IExplorerViewModel _model;
    }
}