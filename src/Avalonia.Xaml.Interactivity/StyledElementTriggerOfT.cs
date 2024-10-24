using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Metadata;

namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// A base class for behaviors, implementing the basic plumbing of <seealso cref="ITrigger"/>.
/// </summary>
/// <typeparam name="T">The object type to attach to</typeparam>
public abstract class StyledElementTrigger<T> : StyledElementTrigger where T : AvaloniaObject
{
    /// <summary>
    /// Gets the object to which this behavior is attached.
    /// </summary>
    public new T? AssociatedObject => base.AssociatedObject as T;

    /// <summary>
    /// Called after the behavior is attached to the <see cref="IBehavior.AssociatedObject"/>.
    /// </summary>
    /// <remarks>
    /// Override this to hook up functionality to the <see cref="IBehavior.AssociatedObject"/>
    /// </remarks>
    /// 
    [RequiresUnreferencedCode("This functionality is not compatible with trimming.")]
    protected override void OnAttached()
    {
        base.OnAttached();

        if (AssociatedObject is null && base.AssociatedObject is not null)
        {
            var actualType = base.AssociatedObject?.GetType().FullName;
            var expectedType = typeof(T).FullName;
            var message = $"AssociatedObject is of type {actualType} but should be of type {expectedType}.";
            throw new InvalidOperationException(message);
        }
    }
}
