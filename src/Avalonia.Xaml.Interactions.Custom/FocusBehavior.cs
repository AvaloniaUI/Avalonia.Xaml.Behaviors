using System.Reactive;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Threading;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class FocusBehavior : DisposingBehavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> IsFocusedProperty =
        AvaloniaProperty.Register<FocusBehavior, bool>(nameof(IsFocused), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// 
    /// </summary>
    public bool IsFocused
    {
        get => GetValue(IsFocusedProperty);
        set => SetValue(IsFocusedProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected override void OnAttached(CompositeDisposable disposables)
    {
		if (AssociatedObject is not null)
		{
			disposables.Add(AssociatedObject.GetObservable(Avalonia.Input.InputElement.IsFocusedProperty)
				.Subscribe(new AnonymousObserver<bool>(
					focused =>
					{
						if (!focused)
						{
							SetCurrentValue(IsFocusedProperty, false);
						}
					})));

			disposables.Add(this.GetObservable(IsFocusedProperty)
				.Subscribe(new AnonymousObserver<bool>(
					focused =>
					{
						if (focused)
						{
                            Dispatcher.UIThread.Post(() => AssociatedObject?.Focus());
						}
					})));
		}
    }
}
