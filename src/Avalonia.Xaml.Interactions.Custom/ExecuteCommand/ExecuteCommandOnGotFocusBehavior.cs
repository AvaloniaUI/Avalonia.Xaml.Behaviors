using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ExecuteCommandOnGotFocusBehavior : ExecuteCommandRoutedEventBehaviorBase
{
    static ExecuteCommandOnGotFocusBehavior()
    {
        EventRoutingStrategyProperty.OverrideMetadata<ExecuteCommandOnGotFocusBehavior>(
            new StyledPropertyMetadata<RoutingStrategies>(
                defaultValue: RoutingStrategies.Tunnel | RoutingStrategies.Bubble));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposable"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
    {
        var control = SourceControl ?? AssociatedObject;
        var dispose = control?
            .AddDisposableHandler(
                InputElement.GotFocusEvent,
                OnGotFocus,
                EventRoutingStrategy);

        if (dispose is not null)
        {
            disposable.Add(dispose);
        }
    }

    private void OnGotFocus(object? sender, RoutedEventArgs e)
    {
        if (e.Handled)
        {
            return;
        }

        if (ExecuteCommand())
        {
            e.Handled = MarkAsHandled;
        }
    }
}
