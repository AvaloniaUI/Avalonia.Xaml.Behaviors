using System;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// An action that displays a <see cref="Popup"/> for the associated control when executed.
/// </summary>
/// <remarks>If the associated control is of type <see cref="Control"/> than popup inherits control <see cref="StyledElement.DataContext"/>.</remarks>
public class PopupAction : AvaloniaObject, IAction
{
    private Popup? _popup;

    /// <summary>
    /// Identifies the <seealso cref="ChildProperty"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<Control?> ChildProperty =
        AvaloniaProperty.Register<PopupAction, Control?>(nameof(Child));

    /// <summary>
    /// Gets or sets the popup Child control. This is a avalonia property.
    /// </summary>
    [Content]
    public Control? Child
    {
        get => GetValue(ChildProperty);
        set => SetValue(ChildProperty, value);
    }

    /// <summary>
    /// Executes the action.
    /// </summary>
    /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
    /// <param name="parameter">The value of this parameter is determined by the caller.</param>
    /// <returns>Returns null after executed.</returns>
    public virtual object? Execute(object? sender, object? parameter)
    {
        if (_popup is null)
        {
            var parent = sender as Control;

            _popup = new Popup()
            {
                Placement = PlacementMode.Pointer,
                PlacementTarget = parent,
                IsLightDismissEnabled = true
            };

            if (sender is Control control)
            {
                BindToDataContext(control, _popup);
            }

            ((ISetLogicalParent)_popup).SetParent(parent);
        }
        _popup.Child = Child;
        _popup.Open();
        return null;
    }

    private static void BindToDataContext(Control source, Control target)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        if (target is null)
        {
            throw new ArgumentNullException(nameof(target));
        }

        var data = source.GetObservable(StyledElement.DataContextProperty);
        if (data is { })
        {
            target.Bind(StyledElement.DataContextProperty, data);
        }
    }
}
