using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Events;

/// <summary>
/// 
/// </summary>
public abstract class ScrollGestureEndedEventBehavior : Behavior<Interactive>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<RoutingStrategies> RoutingStrategiesProperty = 
        AvaloniaProperty.Register<ScrollGestureEndedEventBehavior, RoutingStrategies>(
            nameof(RoutingStrategies),
            RoutingStrategies.Bubble);

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
        AssociatedObject?.AddHandler(Gestures.ScrollGestureEndedEvent, ScrollGestureEnded, RoutingStrategies);
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        AssociatedObject?.RemoveHandler(Gestures.ScrollGestureEndedEvent, ScrollGestureEnded);
    }

    private void ScrollGestureEnded(object? sender, ScrollGestureEventArgs e)
    {
        OnScrollGestureEnded(sender, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnScrollGestureEnded(object? sender, ScrollGestureEventArgs e)
    {
    }
}
