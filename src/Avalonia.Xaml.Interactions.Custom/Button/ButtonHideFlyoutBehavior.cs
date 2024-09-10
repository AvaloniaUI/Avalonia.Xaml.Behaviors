using System;
using System.Reactive.Disposables;
using Avalonia.Controls;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ButtonHideFlyoutBehavior : DisposingBehavior<Button>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> IsFlyoutOpenProperty =
        AvaloniaProperty.Register<ButtonHideFlyoutBehavior, bool>(nameof(IsFlyoutOpen));

    /// <summary>
    /// 
    /// </summary>
    public bool IsFlyoutOpen
    {
        get => GetValue(IsFlyoutOpenProperty);
        set => SetValue(IsFlyoutOpenProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected override void OnAttached(CompositeDisposable disposables)
    {
        var disposable = this.GetObservable(IsFlyoutOpenProperty)
            .Subscribe(isOpen =>
            {
                if (!isOpen)
                {
                    AssociatedObject?.Flyout?.Hide();
                }
            });
        
        disposables.Add(disposable);
    }
}
