﻿using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Sets <see cref="ListBoxItem.IsSelected"/> property to true of the associated <see cref="ListBoxItem"/> control on <see cref="InputElement.PointerMoved"/> event.
/// </summary>
public class SelectListBoxItemOnPointerMovedBehavior : Behavior<Control>
{
    /// <summary>
    /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is { })
        {
            AssociatedObject.PointerMoved += PointerMoved; 
        }
    }

    /// <summary>
    /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
    /// </summary>
    protected override void OnDetaching()
    {
        base.OnDetaching();
        if (AssociatedObject is { })
        {
            AssociatedObject.PointerMoved -= PointerMoved; 
        }
    }

    private void PointerMoved(object? sender, PointerEventArgs args)
    {
        if (AssociatedObject is { } && AssociatedObject.Parent is ListBoxItem item)
        {
            item.IsSelected = true;
            item.Focus();
        }
    }
}