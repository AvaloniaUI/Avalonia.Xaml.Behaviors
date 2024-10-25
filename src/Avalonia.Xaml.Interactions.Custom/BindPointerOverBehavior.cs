using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class BindPointerOverBehavior : DisposingBehavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
	public static readonly StyledProperty<bool> IsPointerOverProperty =
		AvaloniaProperty.Register<BindPointerOverBehavior, bool>(nameof(IsPointerOver), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// 
    /// </summary>
	public bool IsPointerOver
	{
		get => GetValue(IsPointerOverProperty);
		set => SetValue(IsPointerOverProperty, value);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
	protected override void OnAttached(CompositeDisposable disposables)
	{
		if (AssociatedObject is null)
		{
			return;
		}

        var control = AssociatedObject;
        control.PropertyChanged += AssociatedObjectOnPropertyChanged;

        disposables.Add(Disposable.Create(() => control.PropertyChanged -= AssociatedObjectOnPropertyChanged));
		disposables.Add(Disposable.Create(() => IsPointerOver = false));

        return;

        void AssociatedObjectOnPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == InputElement.IsPointerOverProperty)
            {
                IsPointerOver = e.NewValue is true;
            }
        }
	}
}
