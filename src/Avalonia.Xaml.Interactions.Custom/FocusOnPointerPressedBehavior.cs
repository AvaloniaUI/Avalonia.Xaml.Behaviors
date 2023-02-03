using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Focuses the <see cref="Behavior{T}.AssociatedObject"/> on <see cref="InputElement.PointerPressed"/> event.
/// </summary>
public class FocusOnPointerPressedBehavior : Behavior<Control>
{
    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is { })
        {
            AssociatedObject.PointerPressed += PointerPressed; 
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is { })
        {
            AssociatedObject.PointerPressed -= PointerPressed; 
        }
    }

    private void PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        AssociatedObject?.Focus();
    }
}
