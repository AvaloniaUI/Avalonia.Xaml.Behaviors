using System;
using System.Windows.Input;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

internal class Command(Action<object?> execute, Func<object?, bool>? canExecute = null)
    : ICommand
{
    public bool CanExecute(object? parameter)
    {
        return canExecute?.Invoke(parameter) ?? true;
    }

    public void Execute(object? parameter)
    {
        execute.Invoke(parameter);
    }

    #pragma warning disable CS0067
    public event EventHandler? CanExecuteChanged;
    #pragma warning restore CS0067
}
