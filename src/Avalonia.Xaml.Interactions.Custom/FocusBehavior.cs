using System.Reactive;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Data;

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
		base.OnAttached();

		if (AssociatedObject is not null)
		{
			AssociatedObject.AttachedToLogicalTree += (_, _) =>
				disposables.Add(this.GetObservable(IsFocusedProperty)
					.Subscribe(new AnonymousObserver<bool>(
                        focused =>
                        {
                            if (focused)
                            {
                                AssociatedObject.Focus();
                            }
                        })));
		}
	}
}
