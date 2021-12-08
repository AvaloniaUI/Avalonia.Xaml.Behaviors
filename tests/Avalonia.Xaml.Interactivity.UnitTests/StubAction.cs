namespace Avalonia.Xaml.Interactivity.UnitTests;

public class StubAction : AvaloniaObject, IAction
{
    private readonly object? _returnValue;

    public StubAction()
    {
        _returnValue = null;
    }

    public StubAction(object? returnValue)
    {
        this._returnValue = returnValue;
    }

    public object? Sender
    {
        get;
        private set;
    }

    public object? Parameter
    {
        get;
        private set;
    }

    public int ExecuteCount
    {
        get;
        private set;
    }

    public object? Execute(object? sender, object? parameter)
    {
        ExecuteCount++;
        Sender = sender;
        Parameter = parameter;
        return _returnValue;
    }
}