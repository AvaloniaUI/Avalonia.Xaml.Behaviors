using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Sets <see cref="ListBoxItem.IsSelected"/> property to true of the associated <see cref="ListBoxItem"/> control on <see cref="InputElement.PointerMoved"/> event.
/// </summary>
public class SelectListBoxItemOnPointerMovedBehavior : Behavior<Control>
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
        if (AssociatedObject is { Parent: ListBoxItem item })
        {
            item.IsSelected = true;
            item.Focus();
        }
    }
}
