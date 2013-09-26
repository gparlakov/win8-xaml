using System;
using System.Linq;
using System.Windows.Input;

namespace TicTacToe.Behaviour
{
    public delegate bool CanExecuteDelegate<T>(T paramater);

    public delegate void ExecuteDelegate<T>(T paramater);

    public class DelegateCommand<T> : ICommand
    {
        private CanExecuteDelegate<T> canExecute;

        private ExecuteDelegate<T> execute;

        public DelegateCommand(ExecuteDelegate<T> execute) 
            : this(execute, null)
        {  
        }

        public DelegateCommand(ExecuteDelegate<T> execute, CanExecuteDelegate<T> canExecute)
        {
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public bool CanExecute(object parameter)
        {
            if (this.canExecute == null)
            {
                return true;
            }
            return this.canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }

    }
}
