using Avalonia.Input;

namespace Avalonia.Xaml.Interactions.Events;

/// <summary>
/// 
/// </summary>
public abstract class ScrollGestureEndedEventBehavior : InteractiveBehaviorBase
{
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
