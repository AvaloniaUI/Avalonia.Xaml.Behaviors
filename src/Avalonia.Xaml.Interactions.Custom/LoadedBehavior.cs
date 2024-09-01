using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A base class for behaviors using loaded event.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class LoadedBehavior<T> : DisposingBehavior<T> where T : Control
{
    private CompositeDisposable? _disposables;

    /// <inheritdoc />
    protected override void OnAttached(CompositeDisposable disposables)
    {
        _disposables = disposables;
    }

    /// <inheritdoc />
    protected override void OnLoaded()
    {
        OnLoaded(_disposables!);
    }

    /// <summary>
    /// Called after the <see cref="Behavior{T}.AssociatedObject"/> is loaded.
    /// </summary>
    /// <param name="disposable">The group of disposable resources that are disposed together</param>
    protected abstract void OnLoaded(CompositeDisposable disposable);
}
