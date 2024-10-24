using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Focuses the <see cref="IBehavior.AssociatedObject"/> on <see cref="InputElement.PointerMoved"/> event.
/// </summary>
public class FocusOnPointerMovedBehavior : StyledElementBehavior<Control>
{
    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.PointerMoved += PointerMoved;
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.PointerMoved -= PointerMoved;
        }
    }

    private void PointerMoved(object? sender, PointerEventArgs args)
    {
        Dispatcher.UIThread.Post(() => AssociatedObject?.Focus());
    }
}
