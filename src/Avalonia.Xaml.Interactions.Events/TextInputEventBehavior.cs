using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.Events;

/// <summary>
/// 
/// </summary>
public abstract class TextInputEventBehavior : InteractiveBehaviorBase
{
    static TextInputEventBehavior()
    {
        RoutingStrategiesProperty.OverrideMetadata<TextInputEventBehavior>(
            new StyledPropertyMetadata<RoutingStrategies>(
                defaultValue: RoutingStrategies.Tunnel | RoutingStrategies.Bubble));
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        AssociatedObject?.AddHandler(InputElement.TextInputEvent, TextInput, RoutingStrategies);
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        AssociatedObject?.RemoveHandler(InputElement.TextInputEvent, TextInput);
    }

    private void TextInput(object? sender, TextInputEventArgs e)
    {
        OnTextInput(sender, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void OnTextInput(object? sender, TextInputEventArgs e)
    {
    }
}
