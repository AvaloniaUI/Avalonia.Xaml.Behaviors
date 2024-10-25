using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Reactive;
using Avalonia.Threading;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class FocusControlBehavior : AttachedToVisualTreeBehavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> FocusFlagProperty =
        AvaloniaProperty.Register<FocusControlBehavior, bool>(nameof(FocusFlag));

    /// <summary>
    /// 
    /// </summary>
    public bool FocusFlag
    {
        get => GetValue(FocusFlagProperty);
        set => SetValue(FocusFlagProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected override void OnAttached(CompositeDisposable disposables)
    {
        base.OnAttached(disposables);

        var disposable = this.GetObservable(FocusFlagProperty)
            .Subscribe(new AnonymousObserver<bool>(
                focusFlag =>
                {
                    if (focusFlag && IsEnabled)
                    {
                        Dispatcher.UIThread.Post(() => AssociatedObject?.Focus());
                    }
                }));
                
        disposables.Add(disposable);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposable"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
    {
        if (FocusFlag && IsEnabled)
        {
            Dispatcher.UIThread.Post(() => AssociatedObject?.Focus());
        }
    }
}
