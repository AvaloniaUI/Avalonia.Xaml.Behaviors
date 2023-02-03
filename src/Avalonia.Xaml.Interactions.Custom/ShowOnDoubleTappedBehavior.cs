using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A behavior that allows to show control on double tapped event.
/// </summary>
public class ShowOnDoubleTappedBehavior : Behavior<Control>
{
    /// <summary>
    /// Identifies the <seealso cref="TargetControl"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<Control?> TargetControlProperty =
        AvaloniaProperty.Register<ShowOnDoubleTappedBehavior, Control?>(nameof(TargetControl));

    /// <summary>
    /// Gets or sets the target control. This is a avalonia property.
    /// </summary>
    [ResolveByName]
    public Control? TargetControl
    {
        get => GetValue(TargetControlProperty);
        set => SetValue(TargetControlProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        AssociatedObject?.AddHandler(Gestures.DoubleTappedEvent, AssociatedObject_DoubleTapped, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        AssociatedObject?.RemoveHandler(Gestures.DoubleTappedEvent, AssociatedObject_DoubleTapped);
    }

    private void AssociatedObject_DoubleTapped(object? sender, RoutedEventArgs e)
    {
        if (TargetControl is { IsVisible: false })
        {
            TargetControl.IsVisible = true;
            TargetControl.Focus();
        }
    }
}
