using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class HideAttachedFlyoutBehavior : DisposingBehavior<Control>
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
                if (!isOpen && AssociatedObject is not null)
                {
                    FlyoutBase.GetAttachedFlyout(AssociatedObject)?.Hide();
                }
            });

        disposables.Add(disposable);
    }
}
