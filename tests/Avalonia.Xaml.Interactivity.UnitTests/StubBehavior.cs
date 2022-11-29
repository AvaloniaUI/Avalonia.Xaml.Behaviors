using System.Collections.Generic;

namespace Avalonia.Xaml.Interactivity.UnitTests;

public class StubBehavior : AvaloniaObject, IBehavior
{
    public int AttachCount
    {
        get;
        private set;
    }

    public int DetachCount
    {
        get;
        private set;
    }

    public ActionCollection Actions
    {
        get;
        private set;
    }

    public StubBehavior()
    {
        Actions = new ActionCollection();
    }

    public AvaloniaObject? AssociatedObject
    {
        get;
        private set;
    }

    public void Attach(AvaloniaObject? avaloniaObject)
    {
        AssociatedObject = avaloniaObject;
        AttachCount++;
    }

    public void Detach()
    {
        AssociatedObject = null;
        DetachCount++;
    }

    public IEnumerable<object> Execute(object? sender, object parameter)
    {
        return Interaction.ExecuteActions(sender, Actions, parameter);
    }
}
