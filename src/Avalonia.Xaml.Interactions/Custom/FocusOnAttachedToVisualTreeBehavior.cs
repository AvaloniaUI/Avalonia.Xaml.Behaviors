using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Focuses the <see cref="Behavior.AssociatedObject"/> when attached to visual tree.
/// </summary>
public class FocusOnAttachedToVisualTreeBehavior : Behavior<Control>
{
    /// <summary>
    /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();
        if (AssociatedObject is { })
        {
            AssociatedObject.AttachedToVisualTree += AssociatedObject_AttachedToVisualTree; 
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
            AssociatedObject.AttachedToVisualTree -= AssociatedObject_AttachedToVisualTree; 
        }
    }

    private void AssociatedObject_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        AssociatedObject?.Focus();
    }
}