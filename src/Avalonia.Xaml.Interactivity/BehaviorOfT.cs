using System;

namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// A base class for behaviors making them code compatible with older frameworks,
/// and allow for typed associated objects.
/// </summary>
/// <typeparam name="T">The object type to attach to</typeparam>
public abstract class Behavior<T> : Behavior where T : AvaloniaObject
{
    /// <summary>
    /// Gets the object to which this behavior is attached.
    /// </summary>
    public new T? AssociatedObject => base.AssociatedObject as T;

    /// <summary>
    /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
    /// </summary>
    /// <remarks>
    /// Override this to hook up functionality to the <see cref="Behavior.AssociatedObject"/>
    /// </remarks>
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null && base.AssociatedObject is { })
        {
            var actualType = base.AssociatedObject?.GetType().FullName;
            var expectedType = typeof(T).FullName;
            var message = $"AssociatedObject is of type {actualType} but should be of type {expectedType}.";
            throw new InvalidOperationException(message);
        }
    }
}
