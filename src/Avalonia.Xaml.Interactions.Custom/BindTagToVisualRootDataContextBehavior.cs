using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Binds AssociatedObject object Tag property to root visual DataContext.
/// </summary>
public class BindTagToVisualRootDataContextBehavior : DisposingBehavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    /// <exception cref="NotImplementedException"></exception>
    protected override void OnAttached(CompositeDisposable disposables)
    {
        var visualRoot = (Control?)AssociatedObject?.GetVisualRoot();
        if (visualRoot is not null)
        {
            var disposable = BindDataContextToTag(visualRoot, AssociatedObject);
            disposables.Add(disposable);
        }
    }

    private static IDisposable BindDataContextToTag(Control source, Control? target)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        return target.Bind(
            Control.TagProperty, 
            source.GetObservable(StyledElement.DataContextProperty));
    }
}
