using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ExecuteCommandOnPinchEndedBehavior : ExecuteCommandRoutedEventBehaviorBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposable"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
    {
        var control = SourceControl ?? AssociatedObject;
        var dispose = control?
            .AddDisposableHandler(
                Gestures.PinchEndedEvent,
                OnPinchEnded,
                EventRoutingStrategy);

        if (dispose is not null)
        {
            disposable.Add(dispose);
        }
    }

    private void OnPinchEnded(object? sender, RoutedEventArgs e)
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
