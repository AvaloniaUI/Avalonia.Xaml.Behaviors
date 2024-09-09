using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Events;

/// <summary>
/// 
/// </summary>
public abstract class PointerCaptureLostEventBehavior : InteractiveBehaviorBase
{
    static PointerCaptureLostEventBehavior()
    {
        RoutingStrategiesProperty.OverrideMetadata<PointerWheelChangedEventBehavior>(
            new StyledPropertyMetadata<RoutingStrategies>(
                defaultValue: RoutingStrategies.Direct));
    }
    
    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        AssociatedObject?.AddHandler(InputElement.PointerCaptureLostEvent, PointerCaptureLost, RoutingStrategies);
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        AssociatedObject?.RemoveHandler(InputElement.PointerCaptureLostEvent, PointerCaptureLost);
    }

    private void PointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
    {
        OnPointerCaptureLost(sender, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnPointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
    {
    }
}
