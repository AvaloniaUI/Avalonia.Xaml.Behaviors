using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class HorizontalScrollViewerBehavior : Behavior<ScrollViewer>
{
    /// <summary>
    /// 
    /// </summary>
    public enum ChangeSize
    {
        /// <summary>
        /// 
        /// </summary>
        Line,

        /// <summary>
        /// 
        /// </summary>
        Page
    }

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<HorizontalScrollViewerBehavior, bool>(nameof(IsEnabled), true);

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> RequireShiftKeyProperty =
        AvaloniaProperty.Register<HorizontalScrollViewerBehavior, bool>(nameof(RequireShiftKey));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<ChangeSize> ScrollChangeSizeProperty =
        AvaloniaProperty.Register<HorizontalScrollViewerBehavior, ChangeSize>(nameof(ScrollChangeSize));

    /// <summary>
    /// 
    /// </summary>
    public bool IsEnabled
    {
        get => GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public bool RequireShiftKey
    {
        get => GetValue(RequireShiftKeyProperty);
        set => SetValue(RequireShiftKeyProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public ChangeSize ScrollChangeSize
    {
        get => GetValue(ScrollChangeSizeProperty);
        set => SetValue(ScrollChangeSizeProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();

        AssociatedObject!.AddHandler(InputElement.PointerWheelChangedEvent, OnPointerWheelChanged,
            RoutingStrategies.Tunnel);
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void OnDetaching()
    {
        base.OnDetaching();

        AssociatedObject!.RemoveHandler(InputElement.PointerWheelChangedEvent, OnPointerWheelChanged);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnPointerWheelChanged(object? sender, PointerWheelEventArgs e)
    {
        if (!IsEnabled)
        {
            e.Handled = true;
            return;
        }

        if (RequireShiftKey && e.KeyModifiers == KeyModifiers.Shift || !RequireShiftKey)
        {
            if (e.Delta.Y < 0)
            {
                if (ScrollChangeSize == ChangeSize.Line)
                {
                    AssociatedObject!.LineRight();
                }
                else
                {
                    AssociatedObject!.PageRight();
                }
            }
            else
            {
                if (ScrollChangeSize == ChangeSize.Line)
                {
                    AssociatedObject!.LineLeft();
                }
                else
                {
                    AssociatedObject!.PageLeft();
                }
            }
        }
    }
}
