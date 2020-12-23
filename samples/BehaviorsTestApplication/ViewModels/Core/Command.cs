using System;
using System.Windows.Input;

namespace BehaviorsTestApplication.ViewModels.Core
{
    public class Command : ICommand
    {
        private Action<object?>? _execute;
        private Predicate<object?>? _canExecute;

        public event EventHandler? CanExecuteChanged;

        public Command(Action<object?>? execute = null, Predicate<object?>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public virtual void NotifyCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }

        public void Execute(object? parameter)
        {
            _execute?.Invoke(parameter);
        }
    }
}
