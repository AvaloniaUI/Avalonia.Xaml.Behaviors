using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Events;

/// <summary>
/// 
/// </summary>
public abstract class PointerExitedEventBehavior : InteractiveBehaviorBase
{
    static PointerExitedEventBehavior()
    {
        RoutingStrategiesProperty.OverrideMetadata<PointerExitedEventBehavior>(
            new StyledPropertyMetadata<RoutingStrategies>(
                defaultValue: RoutingStrategies.Direct));
    }


    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        AssociatedObject?.AddHandler(InputElement.PointerExitedEvent, PointerLeave, RoutingStrategies);
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        AssociatedObject?.RemoveHandler(InputElement.PointerExitedEvent, PointerLeave);
    }

    private void PointerLeave(object? sender, PointerEventArgs e)
    {
        OnPointerLeave(sender, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnPointerLeave(object? sender, PointerEventArgs e)
    {
    }
}
