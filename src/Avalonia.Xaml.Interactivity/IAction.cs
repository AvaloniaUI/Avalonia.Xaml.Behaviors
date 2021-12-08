namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// Interface implemented by all custom actions.
/// </summary>
public interface IAction
{
    /// <summary>
    /// Executes the action.
    /// </summary>
    /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
    /// <param name="parameter">The value of this parameter is determined by the caller.</param>
    /// <remarks> An example of parameter usage is EventTriggerBehavior, which passes the EventArgs as a parameter to its actions.</remarks>
    /// <returns>Returns the result of the action.</returns>
    object? Execute(object? sender, object? parameter);
}