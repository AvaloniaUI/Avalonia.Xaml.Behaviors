using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ButtonExecuteCommandOnKeyDownBehavior : AttachedToVisualTreeBehavior<Button>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<ButtonExecuteCommandOnKeyDownBehavior, bool>(nameof(IsEnabled));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<Key?> KeyProperty =
        AvaloniaProperty.Register<ButtonExecuteCommandOnKeyDownBehavior, Key?>(nameof(Key));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<KeyGesture?> GestureProperty =
        AvaloniaProperty.Register<ButtonExecuteCommandOnKeyDownBehavior, KeyGesture?>(nameof(Gesture));

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
    /// <param name="disposables"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposables)
    {
        if (AssociatedObject?.GetVisualRoot() is InputElement inputRoot)
        {
            var disposable = inputRoot.AddDisposableHandler(InputElement.KeyDownEvent, RootDefaultKeyDown);
            disposables.Add(disposable);
        }
    }

    private void RootDefaultKeyDown(object? sender, KeyEventArgs e)
    {
        var haveKey = Key is not null && e.Key == Key;
        var haveGesture = Gesture is not null && Gesture.Matches(e);

        if (!haveKey && !haveGesture)
        {
            return;
        }

        if (AssociatedObject is { } button)
        {
            ExecuteCommand(button);
        }
    }
  
    private bool ExecuteCommand(Button button)
    {
        if (!IsEnabled)
        {
            return false;
        }

        if (button is not { IsVisible: true, IsEnabled: true })
        {
            return false;
        }

        if (button.Command?.CanExecute(button.CommandParameter) != true)
        {
            return false;
        }

        button.Command.Execute(button.CommandParameter);
        return true;
    }
}
