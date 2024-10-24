using System;
using System.Diagnostics;
using System.Globalization;
using System.Reactive.Linq;
using Avalonia.Controls;

namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// A base class for behaviors, implementing the basic plumbing of <see cref="IBehavior"/>.
/// </summary>
public abstract class Behavior : AvaloniaObject, IBehavior
{
    private AvaloniaObject? _associatedObject;

    private readonly IDisposable _associatedDataContextUpdater;

    /// <summary>
    /// <see cref="AvaloniaProperty"/> associated to <see cref="AssociatedObject"/>
    /// </summary>
    public static readonly DirectProperty<Behavior, AvaloniaObject?> AssociatedObjectProperty = AvaloniaProperty.RegisterDirect<Behavior, AvaloniaObject?>(
        nameof(AssociatedObject), o => o.AssociatedObject, (o, v) => o.AssociatedObject = v);

    /// <summary>
    /// Avalonia Property associated to <see cref="AssociatedDataContext"/>
    /// </summary>
    public static readonly StyledProperty<object?> AssociatedDataContextProperty = AvaloniaProperty.Register<Behavior, object?>(
        nameof(AssociatedDataContext));

    /// <summary>
    /// Initialize the Behavior
    /// </summary>
    protected Behavior()
    {
        _associatedDataContextUpdater = this.GetObservable(AssociatedObjectProperty)
            .Where(o => o != null).Select(o => o!)
            .OfType<StyledElement>().Select(o => AssociatedDataContext = o.DataContext)
            .Subscribe();
    }

    /// <summary>
    /// Gets the DataContext of the <see cref="AssociatedObject"/>
    /// </summary>
    public object? AssociatedDataContext
    {
        get => GetValue(AssociatedDataContextProperty);
        private set => SetValue(AssociatedDataContextProperty, value);
    }

    /// <summary>
    /// Gets the <see cref="AvaloniaObject"/> to which the behavior is attached.
    /// </summary>
    public AvaloniaObject? AssociatedObject
    {
        get => _associatedObject;
        private set => SetAndRaise(AssociatedObjectProperty, ref _associatedObject, value);
    }

    /// <summary>
    /// Attaches the behavior to the specified <see cref="AvaloniaObject"/>.
    /// </summary>
    /// <param name="associatedObject">The <see cref="AvaloniaObject"/> to which to attach.</param>
    /// <exception cref="ArgumentNullException"><paramref name="associatedObject"/> is null.</exception>
    public void Attach(AvaloniaObject? associatedObject)
    {
        if (Equals(associatedObject, AssociatedObject))
        {
            return;
        }

        if (AssociatedObject is not null)
        {
            throw new InvalidOperationException(string.Format(
                CultureInfo.CurrentCulture,
                "An instance of a behavior cannot be attached to more than one object at a time."));
        }

        Debug.Assert(associatedObject is not null, "Cannot attach the behavior to a null object.");
        AssociatedObject = associatedObject ?? throw new ArgumentNullException(nameof(associatedObject));

        OnAttached();
    }

    /// <summary>
    /// Detaches the behaviors from the <see cref="AssociatedObject"/>.
    /// </summary>
    public void Detach()
    {
        OnDetaching();
        AssociatedObject = null;
        _associatedDataContextUpdater.Dispose();
    }

    /// <summary>
    /// Called after the behavior is attached to the <see cref="AssociatedObject"/>.
    /// </summary>
    /// <remarks>
    /// Override this to hook up functionality to the <see cref="AssociatedObject"/>
    /// </remarks>
    protected virtual void OnAttached()
    {
    }

    /// <summary>
    /// Called when the behavior is being detached from its <see cref="AssociatedObject"/>.
    /// </summary>
    /// <remarks>
    /// Override this to unhook functionality from the <see cref="AssociatedObject"/>
    /// </remarks>
    protected virtual void OnDetaching()
    {
    }

    internal void AttachedToVisualTree()
    {
        OnAttachedToVisualTree();
    }

    internal void DetachedFromVisualTree()
    {
        OnDetachedFromVisualTree();
    }

    /// <summary>
    /// Called after the <see cref="AssociatedObject"/> is attached to the visual tree.
    /// </summary>
    /// <remarks>
    /// Invoked only when the <see cref="AssociatedObject"/> is of type <see cref="Control"/>.
    /// </remarks>
    protected virtual void OnAttachedToVisualTree()
    {
    }

    /// <summary>
    /// Called when the <see cref="AssociatedObject"/> is being detached from the visual tree.
    /// </summary>
    /// <remarks>
    /// Invoked only when the <see cref="AssociatedObject"/> is of type <see cref="Control"/>.
    /// </remarks>
    protected virtual void OnDetachedFromVisualTree()
    {
    }
}
