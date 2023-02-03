using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Focuses the <see cref="Behavior.AssociatedObject"/> when attached to visual tree.
/// </summary>
public class FocusOnAttachedToVisualTreeBehavior : Behavior<Control>
{
    /// <inheritdoc/>
    protected override void OnAttachedToVisualTree()
    {
        AssociatedObject?.Focus();
    }
}
