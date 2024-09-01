namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// A base class for action that calls a method on a specified object when invoked.
/// </summary>
public abstract class Action : AvaloniaObject, IAction
{
    /// <summary>
    /// Identifies the <seealso cref="IsEnabled"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<Avalonia.Xaml.Interactivity.Action, bool>(nameof(IsEnabled), defaultValue: true);

    /// <summary>
    /// Gets or sets a value indicating whether this instance is enabled.
    /// </summary>
    /// <value><c>true</c> if this instance is enabled; otherwise, <c>false</c>.</value>
    public bool IsEnabled
    {
        get => GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }
    
    /// <summary>
    /// Executes the action.
    /// </summary>
    /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
    /// <param name="parameter">The value of this parameter is determined by the caller.</param>
    /// <remarks> An example of parameter usage is EventTriggerBehavior, which passes the EventArgs as a parameter to its actions.</remarks>
    /// <returns>Returns the result of the action.</returns>
    public abstract object? Execute(object? sender, object? parameter);
}
