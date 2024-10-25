using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.Events;

/// <summary>
/// 
/// </summary>
public abstract class PointerReleasedEventBehavior : InteractiveBehaviorBase
{
    static PointerReleasedEventBehavior()
    {
        RoutingStrategiesProperty.OverrideMetadata<PointerReleasedEventBehavior>(
            new StyledPropertyMetadata<RoutingStrategies>(
                defaultValue: RoutingStrategies.Tunnel | RoutingStrategies.Bubble));
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        AssociatedObject?.AddHandler(InputElement.PointerReleasedEvent, PointerReleased, RoutingStrategies);
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        AssociatedObject?.RemoveHandler(InputElement.PointerReleasedEvent, PointerReleased);
    }

    private void PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        OnPointerReleased(sender, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
    }
}
