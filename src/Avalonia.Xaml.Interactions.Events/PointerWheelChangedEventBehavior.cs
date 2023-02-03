using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Events;

/// <summary>
/// 
/// </summary>
public abstract class PointerWheelChangedEventBehavior : Behavior<Interactive>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<RoutingStrategies> RoutingStrategiesProperty = 
        AvaloniaProperty.Register<PointerWheelChangedEventBehavior, RoutingStrategies>(
            nameof(RoutingStrategies),
            RoutingStrategies.Tunnel | RoutingStrategies.Bubble);

    /// <summary>
    /// 
    /// </summary>
    public RoutingStrategies RoutingStrategies
    {
        get => GetValue(RoutingStrategiesProperty);
        set => SetValue(RoutingStrategiesProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        AssociatedObject?.AddHandler(InputElement.PointerWheelChangedEvent, PointerWheelChanged, RoutingStrategies);
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        AssociatedObject?.RemoveHandler(InputElement.PointerWheelChangedEvent, PointerWheelChanged);
    }

    private void PointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        OnPointerWheelChanged(sender, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
    }
}
