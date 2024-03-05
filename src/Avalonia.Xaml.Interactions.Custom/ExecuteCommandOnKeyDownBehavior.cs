using System.Reactive.Disposables;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ExecuteCommandOnKeyDownBehavior : AttachedToVisualTreeBehavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<Key?> KeyProperty =
        AvaloniaProperty.Register<ExecuteCommandOnKeyDownBehavior, Key?>(nameof(Key));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<ExecuteCommandOnKeyDownBehavior, bool>(nameof(IsEnabled), true);

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ExecuteCommandOnKeyDownBehavior, ICommand>(nameof(Command));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<object> CommandParameterProperty =
        AvaloniaProperty.Register<ExecuteCommandOnKeyDownBehavior, object>(nameof(CommandParameter));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<RoutingStrategies> EventRoutingStrategyProperty =
        AvaloniaProperty.Register<ExecuteCommandOnKeyDownBehavior, RoutingStrategies>(nameof(EventRoutingStrategy),
            RoutingStrategies.Bubble);

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
    public bool IsEnabled
    {
        get => GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
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
    /// <param name="disposables"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposables)
    {
        var control = AssociatedObject;
        if (control is null)
        {
            return;
        }

        if (control.GetVisualRoot() is InputElement inputRoot)
        {
            var disposable =
                inputRoot.AddDisposableHandler(InputElement.KeyDownEvent, RootDefaultKeyDown, EventRoutingStrategy);
            disposables.Add(disposable);
        }
    }

    private void RootDefaultKeyDown(object? sender, KeyEventArgs e)
    {
        var control = AssociatedObject;
        if (control is null)
        {
            return;
        }

        if (Key is { } && e.Key == Key && control.IsVisible && control.IsEnabled && IsEnabled)
        {
            if (!e.Handled && Command?.CanExecute(CommandParameter) == true)
            {
                Command.Execute(CommandParameter);
                e.Handled = true;
            }
        }
    }
}
