namespace Avalonia.Xaml.Interactivity.UnitTests;

public class StubAction(object? returnValue) : Avalonia.Xaml.Interactivity.Action
{
    public StubAction() : this(null)
    {
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

    public override object? Execute(object? sender, object? parameter)
    {
        ExecuteCount++;
        Sender = sender;
        Parameter = parameter;
        return returnValue;
    }
}
