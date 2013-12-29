using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace BuildingEditor.ViewModel
{
    public class SimpleCommand : ICommand
    {
        public SimpleCommand() { }

        public SimpleCommand(Action<object> execute)
            : this()
        {
            ExecuteMethod = execute;
        }

        public SimpleCommand(Action<object> execute, Predicate<object> canExecute)
            : this(execute)
        {
            CanExecuteMethod = canExecute;
        }

        public Action<object> ExecuteMethod { get; set; }
        public Predicate<object> CanExecuteMethod { get; set; }

        public bool CanExecute(object parameter)
        {
            if (CanExecuteMethod != null)
                return CanExecuteMethod(parameter);
            return true;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            if (ExecuteMethod != null)
                ExecuteMethod(parameter);
        }
    }
}
