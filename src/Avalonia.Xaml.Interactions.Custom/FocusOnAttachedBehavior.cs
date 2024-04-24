using System.Reactive.Disposables;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class FocusOnAttachedBehavior : FocusBehaviorBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposables)
    {
        Focus();
    }
}
