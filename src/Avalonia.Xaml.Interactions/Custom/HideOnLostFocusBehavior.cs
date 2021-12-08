using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A behavior that allows to hide control on lost focus event.
/// </summary>
public class HideOnLostFocusBehavior : Behavior<Control>
{
    /// <summary>
    /// Identifies the <seealso cref="TargetControl"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<Control?> TargetControlProperty =
        AvaloniaProperty.Register<HideOnLostFocusBehavior, Control?>(nameof(TargetControl));

    /// <summary>
    /// Gets or sets the target control. This is a avalonia property.
    /// </summary>
    public Control? TargetControl
    {
        get => GetValue(TargetControlProperty);
        set => SetValue(TargetControlProperty, value);
    }

    /// <summary>
    /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();
        AssociatedObject?.AddHandler(InputElement.LostFocusEvent, AssociatedObject_LostFocus, RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
    }

    /// <summary>
    /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
    /// </summary>
    protected override void OnDetaching()
    {
        base.OnDetaching();
        AssociatedObject?.RemoveHandler(InputElement.LostFocusEvent, AssociatedObject_LostFocus);
    }

    private void AssociatedObject_LostFocus(object? sender, RoutedEventArgs e)
    {
        if (TargetControl is { })
        {
            TargetControl.IsVisible = false;
        }
    }
}