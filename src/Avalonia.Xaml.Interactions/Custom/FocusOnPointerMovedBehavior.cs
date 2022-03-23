using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Focuses the <see cref="Behavior.AssociatedObject"/> on <see cref="InputElement.PointerMoved"/> event.
/// </summary>
public class FocusOnPointerMovedBehavior : Behavior<Control>
{
    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is { })
        {
            AssociatedObject.PointerMoved += PointerMoved; 
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is { })
        {
            AssociatedObject.PointerMoved -= PointerMoved; 
        }
    }

    private void PointerMoved(object? sender, PointerEventArgs args)
    {
        AssociatedObject?.Focus();
    }
}
