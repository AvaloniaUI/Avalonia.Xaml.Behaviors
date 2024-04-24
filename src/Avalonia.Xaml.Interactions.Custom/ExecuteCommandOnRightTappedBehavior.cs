using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ExecuteCommandOnRightTappedBehavior : ExecuteCommandBehaviorBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposable"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
    {
        var dispose = AssociatedObject?
            .AddDisposableHandler(
                Gestures.RightTappedEvent,
                AssociatedObject_RightTapped,
                RoutingStrategies.Tunnel | RoutingStrategies.Bubble);

        if (dispose is not null)
        {
            disposable.Add(dispose);
        }
    }

    private void AssociatedObject_RightTapped(object? sender, RoutedEventArgs e)
    {
        if (e.Handled)
        {
            return;
        }

        if (ExecuteCommand())
        {
            e.Handled = true;
        }
    }
}
