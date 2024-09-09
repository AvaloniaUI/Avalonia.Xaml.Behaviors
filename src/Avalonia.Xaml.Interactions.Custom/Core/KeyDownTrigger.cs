using System.Reactive.Disposables;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class KeyDownTrigger : RoutedEventTriggerBase
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<Key> KeyProperty =
        AvaloniaProperty.Register<KeyDownTrigger, Key>(nameof(Key));

    /// <summary>
    /// 
    /// </summary>
    public Key Key
    {
        get => GetValue(KeyProperty);
        set => SetValue(KeyProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public bool MarkAsHandled { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected override void OnAttached(CompositeDisposable disposables)
    {
        if (AssociatedObject is InputElement element)
        {
            var disposable = element.AddDisposableHandler(InputElement.KeyDownEvent, OnKeyDown, EventRoutingStrategy);
            disposables.Add(disposable);
        }
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (!IsEnabled)
        {
            return;
        }

        if (e.Key == Key)
        {
            e.Handled = MarkAsHandled;
            Interaction.ExecuteActions(AssociatedObject, Actions, null);
        }
    }
}
