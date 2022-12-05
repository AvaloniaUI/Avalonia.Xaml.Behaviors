using System;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Binds AssociatedObject object Tag property to root visual DataContext.
/// </summary>
public class BindTagToVisualRootDataContextBehavior : Behavior<Control>
{
    private IDisposable? _disposable;

    /// <inheritdoc/>
    protected override void OnAttachedToVisualTree()
    {
        var visualRoot = (Control?)AssociatedObject?.GetVisualRoot();
        if (visualRoot is { })
        {
            _disposable = BindDataContextToTag(visualRoot, AssociatedObject);
        }
    }

    /// <inheritdoc/>
    protected override void OnDetachedFromVisualTree()
    {
        _disposable?.Dispose();
    }

    private static IDisposable? BindDataContextToTag(Control source, Control? target)
    {
        if (source is null)
            throw new ArgumentNullException(nameof(source));

        if (target is null)
            throw new ArgumentNullException(nameof(target));

        var data = source.GetObservable(StyledElement.DataContextProperty);
        return data is { } ? target.Bind(Control.TagProperty, data) : null;
    }
}
