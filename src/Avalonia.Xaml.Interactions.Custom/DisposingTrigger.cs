using System.Reactive.Disposables;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class DisposingTrigger : Trigger
{
    private readonly CompositeDisposable _disposables = new();

    /// <summary>
    /// 
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();

        OnAttached(_disposables);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected abstract void OnAttached(CompositeDisposable disposables);

    /// <summary>
    /// 
    /// </summary>
    protected override void OnDetaching()
    {
        base.OnDetaching();

        _disposables.Dispose();
    }
}
