using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace CareConnect.ViewModel.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> execute;
        private readonly Func<object?, bool>? canExecute;
        public event EventHandler? CanExecuteChanged;
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }
        public bool CanExecute(object? parameter)
        {
            if (canExecute != null)
                return canExecute.Invoke(parameter);
            else
                return true;
        }
        public void Execute(object? parameter)
        {
            execute(parameter);
        }
        public void Refresh()
        {
            if (CanExecuteChanged != null)
                CanExecuteChanged.Invoke(this, EventArgs.Empty);
        }
    }

}
