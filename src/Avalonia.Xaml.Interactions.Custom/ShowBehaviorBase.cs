using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class ShowBehaviorBase : AttachedToVisualTreeBehavior<Control>
{
    /// <summary>
    /// Identifies the <seealso cref="TargetControl"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<Control?> TargetControlProperty =
        AvaloniaProperty.Register<ShowBehaviorBase, Control?>(nameof(TargetControl));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<RoutingStrategies> EventRoutingStrategyProperty =
        AvaloniaProperty.Register<ShowBehaviorBase, RoutingStrategies>(nameof(EventRoutingStrategy), RoutingStrategies.Bubble);

    /// <summary>
    /// Gets or sets the target control. This is a avalonia property.
    /// </summary>
    [ResolveByName]
    public Control? TargetControl
    {
        get => GetValue(TargetControlProperty);
        set => SetValue(TargetControlProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public RoutingStrategies EventRoutingStrategy
    {
        get => GetValue(EventRoutingStrategyProperty);
        set => SetValue(EventRoutingStrategyProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected bool Show()
    {
        if (IsEnabled && TargetControl is { IsVisible: false })
        {
            TargetControl.SetCurrentValue(Visual.IsVisibleProperty, true);

            Dispatcher.UIThread.Post(() => TargetControl.Focus());

            return true;
        }

        return false;
    }
}
