using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A behavior that allows to show control on key down event.
/// </summary>
public class ShowOnKeyDownBehavior : ShowBehaviorBase
{
    /// <summary>
    /// Identifies the <seealso cref="Key"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<Key?> KeyProperty =
        AvaloniaProperty.Register<ShowOnKeyDownBehavior, Key?>(nameof(Key));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<KeyGesture?> GestureProperty =
        AvaloniaProperty.Register<ShowOnKeyDownBehavior, KeyGesture?>(nameof(Gesture));

    /// <summary>
    /// Gets or sets the key. This is a avalonia property.
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposable"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
    {
        var dispose = AssociatedObject?
            .AddDisposableHandler(
                InputElement.KeyDownEvent, 
                AssociatedObject_KeyDown, 
                RoutingStrategies.Tunnel | RoutingStrategies.Bubble);

        if (dispose is not null)
        {
            disposable.Add(dispose);
        }
    }

    private void AssociatedObject_KeyDown(object? sender, KeyEventArgs e)
    {
        var haveKey = Key is not null && e.Key == Key;
        var haveGesture = Gesture is not null && Gesture.Matches(e);

        if (!haveKey && !haveGesture)
        {
            return;
        }
        
        Show();
    }
}
