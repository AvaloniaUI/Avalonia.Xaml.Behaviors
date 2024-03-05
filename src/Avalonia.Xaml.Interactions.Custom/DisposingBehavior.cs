using System.Reactive.Disposables;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A base class for behaviors with disposable resources.
/// </summary>
/// <typeparam name="T">The object type to attach to</typeparam>
public abstract class DisposingBehavior<T> : Behavior<T> where T : AvaloniaObject
{
	private CompositeDisposable? _disposables;

    /// <inheritdoc />
	protected override void OnAttached()
	{
		base.OnAttached();

		_disposables?.Dispose();

		_disposables = new CompositeDisposable();

		OnAttached(_disposables);
	}

    /// <summary>
    /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
    /// </summary>
    /// <param name="disposables">The group of disposable resources that are disposed together.</param>
	protected abstract void OnAttached(CompositeDisposable disposables);

    /// <inheritdoc />
	protected override void OnDetaching()
	{
		base.OnDetaching();

		_disposables?.Dispose();
	}
}
