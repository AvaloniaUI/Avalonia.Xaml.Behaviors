using Avalonia.Input;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public abstract class ExecuteCommandOnKeyBehaviorBase : ExecuteCommandRoutedEventBehaviorBase
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<Key?> KeyProperty =
        AvaloniaProperty.Register<ExecuteCommandOnKeyBehaviorBase, Key?>(nameof(Key));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<KeyGesture?> GestureProperty =
        AvaloniaProperty.Register<ExecuteCommandOnKeyBehaviorBase, KeyGesture?>(nameof(Gesture));

    /// <summary>
    /// 
    /// </summary>
    public Key? Key
    {
        get => GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public KeyGesture? Gesture
    {
        get => GetValue(GestureProperty);
        set => SetValue(GestureProperty, value);
    }
}
