using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A behavior that listens for a <see cref="Button.ClickEvent"/> event on its source and executes its actions when that event is fired.
/// </summary>
public class ButtonClickEventTriggerBehavior : Trigger<Button>
{
    private KeyModifiers _savedKeyModifiers = KeyModifiers.None;

    /// <summary>
    /// Identifies the <seealso cref="KeyModifiers"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<KeyModifiers> KeyModifiersProperty =
        AvaloniaProperty.Register<ButtonClickEventTriggerBehavior, KeyModifiers>(nameof(KeyModifiers));

    /// <summary>
    /// Gets or sets the required key modifiers to execute <see cref="Button.ClickEvent"/> event handler. This is a avalonia property.
    /// </summary>
    public KeyModifiers KeyModifiers
    {
        get => GetValue(KeyModifiersProperty);
        set => SetValue(KeyModifiersProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is { })
        {
            AssociatedObject.Click += AssociatedObject_OnClick;
            AssociatedObject.AddHandler(InputElement.KeyDownEvent, Button_OnKeyDown, RoutingStrategies.Tunnel);
            AssociatedObject.AddHandler(InputElement.KeyUpEvent, Button_OnKeyUp, RoutingStrategies.Tunnel);
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is { })
        {
            AssociatedObject.Click -= AssociatedObject_OnClick;
            AssociatedObject.RemoveHandler(InputElement.KeyDownEvent, Button_OnKeyDown);
            AssociatedObject.RemoveHandler(InputElement.KeyUpEvent, Button_OnKeyUp);
        }
    }

    private void AssociatedObject_OnClick(object? sender, RoutedEventArgs e)
    {
        if (AssociatedObject is { } && KeyModifiers == _savedKeyModifiers)
        {
            Interaction.ExecuteActions(AssociatedObject, Actions, e);
        }
    }

    private void Button_OnKeyDown(object? sender, KeyEventArgs e)
    {
        _savedKeyModifiers = e.KeyModifiers;
    }

    private void Button_OnKeyUp(object? sender, KeyEventArgs e)
    {
        _savedKeyModifiers = KeyModifiers.None;
    }
}
