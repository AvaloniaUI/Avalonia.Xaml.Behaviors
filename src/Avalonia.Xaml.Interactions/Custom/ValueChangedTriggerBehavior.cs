using System;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A behavior that performs actions when the bound data produces new value.
/// </summary>
public class ValueChangedTriggerBehavior : Trigger
{
    static ValueChangedTriggerBehavior()
    {
        BindingProperty.Changed.Subscribe(e => OnValueChanged(e.Sender, e));
    }

    /// <summary>
    /// Identifies the <seealso cref="Binding"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<object?> BindingProperty =
        AvaloniaProperty.Register<ValueChangedTriggerBehavior, object?>(nameof(Binding));

    /// <summary>
    /// Gets or sets the bound object that the <see cref="ValueChangedTriggerBehavior"/> will listen to. This is a avalonia property.
    /// </summary>
    public object? Binding
    {
        get => GetValue(BindingProperty);
        set => SetValue(BindingProperty, value);
    }

    private static void OnValueChanged(IAvaloniaObject avaloniaObject, AvaloniaPropertyChangedEventArgs args)
    {
        if (avaloniaObject is not ValueChangedTriggerBehavior behavior || behavior.AssociatedObject is null)
        {
            return;
        }

        var binding = behavior.Binding;
        if (binding is { })
        {
            Interaction.ExecuteActions(behavior.AssociatedObject, behavior.Actions, args);
        }
    }
}