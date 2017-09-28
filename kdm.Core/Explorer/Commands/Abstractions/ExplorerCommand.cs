using kmd.Core.Explorer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace kdm.Core.Explorer.Commands.Abstractions
{
    public abstract class ExplorerCommand : ICommand
    {
        public IExplorerModel Model
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

        private IExplorerModel _model;

        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);

        public abstract void Execute(object parameter);
    }
}