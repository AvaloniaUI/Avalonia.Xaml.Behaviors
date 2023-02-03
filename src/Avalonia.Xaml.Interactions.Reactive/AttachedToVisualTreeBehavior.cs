using System.Reactive.Disposables;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A base class for behaviors using attached to visual tree event.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class AttachedToVisualTreeBehavior<T> : DisposingBehavior<T> where T : Visual
{
	private CompositeDisposable? _disposables;

    /// <inheritdoc />
	protected override void OnAttached(CompositeDisposable disposables)
	{
		_disposables = disposables;
	}

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
		OnAttachedToVisualTree(_disposables!);
    }

    /// <summary>
    /// Called after the behavior is attached to the <see cref="Behavior{T}.AssociatedObject"/> visual tree.
    /// </summary>
    /// <param name="disposable">The group of disposable resources that are disposed together</param>
	protected abstract void OnAttachedToVisualTree(CompositeDisposable disposable);
}
