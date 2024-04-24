using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Threading;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class FocusBehaviorBase : AttachedToVisualTreeBehavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<FocusBehaviorBase, bool>(nameof(IsEnabled), true);

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<NavigationMethod> NavigationMethodProperty =
        AvaloniaProperty.Register<FocusBehaviorBase, NavigationMethod>(nameof(NavigationMethod));
    
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<KeyModifiers> KeyModifiersProperty =
        AvaloniaProperty.Register<FocusBehaviorBase, KeyModifiers>(nameof(KeyModifiers));

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
    public NavigationMethod NavigationMethod
    {
        get => GetValue(NavigationMethodProperty);
        set => SetValue(NavigationMethodProperty, value);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public KeyModifiers KeyModifiers
    {
        get => GetValue(KeyModifiersProperty);
        set => SetValue(KeyModifiersProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    protected virtual bool Focus()
    {
        if (!IsEnabled)
        {
            return false;
        }

        Dispatcher.UIThread.Post(() => AssociatedObject?.Focus(NavigationMethod, KeyModifiers));

        return true;

    }
}
