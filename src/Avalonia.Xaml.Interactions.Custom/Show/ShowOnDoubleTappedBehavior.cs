using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A behavior that allows to show control on double tapped event.
/// </summary>
public class ShowOnDoubleTappedBehavior : ShowBehaviorBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposable"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
    {
        var dispose = AssociatedObject?
            .AddDisposableHandler(
                Gestures.DoubleTappedEvent, 
                AssociatedObject_DoubleTapped, 
                EventRoutingStrategy);

        if (dispose is not null)
        {
            disposable.Add(dispose);
        }
    }

    private void AssociatedObject_DoubleTapped(object? sender, RoutedEventArgs e)
    {
        Show();
    }
}
