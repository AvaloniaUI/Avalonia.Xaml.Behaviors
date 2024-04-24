using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ExecuteCommandOnTextInputMethodClientRequestedBehavior : ExecuteCommandBehaviorBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposable"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
    {
        var dispose = AssociatedObject?
            .AddDisposableHandler(
                InputElement.TextInputMethodClientRequestedEvent,
                AssociatedObject_TextInputMethodClientRequested,
                RoutingStrategies.Tunnel | RoutingStrategies.Bubble);

        if (dispose is not null)
        {
            disposable.Add(dispose);
        }
    }

    private void AssociatedObject_TextInputMethodClientRequested(object? sender, RoutedEventArgs e)
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
