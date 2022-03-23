namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// Interface implemented by all custom triggers.
/// </summary>
public interface ITrigger : IBehavior
{
    /// <summary>
    /// Gets the collection of actions associated with the behavior.
    /// </summary>
    ActionCollection Actions { get; }
}
