using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Focuses the <see cref="StyledElementBehavior{T}.AssociatedObject"/> on <see cref="InputElement.PointerPressed"/> event.
/// </summary>
public class FocusOnPointerPressedBehavior : StyledElementBehavior<Control>
{
    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.PointerPressed += PointerPressed;
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.PointerPressed -= PointerPressed;
        }
    }

    private void PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        Dispatcher.UIThread.Post(() => AssociatedObject?.Focus());
    }
}
