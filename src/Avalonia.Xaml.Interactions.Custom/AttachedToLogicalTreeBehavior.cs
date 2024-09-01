using System.Reactive.Disposables;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A base class for behaviors using attached to logical tree event.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AttachedToLogicalTreeBehavior<T> : DisposingBehavior<T> where T : StyledElement
{
    private CompositeDisposable? _disposables;

    /// <inheritdoc />
    protected override void OnAttached(CompositeDisposable disposables)
    {
        _disposables = disposables;
    }

    /// <inheritdoc />
    protected override void OnAttachedToLogicalTree()
    {
        OnAttachedToLogicalTree(_disposables!);
    }

    /// <summary>
    /// Called after the <see cref="Behavior{T}.AssociatedObject"/> is attached to the logical tree.
    /// </summary>
    /// <param name="disposable">The group of disposable resources that are disposed together</param>
    protected abstract void OnAttachedToLogicalTree(CompositeDisposable disposable);
}
