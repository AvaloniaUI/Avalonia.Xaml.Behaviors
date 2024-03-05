using System.Reactive;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Data;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class BoundsObserverBehavior : DisposingBehavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
	public static readonly StyledProperty<Rect> BoundsProperty =
		AvaloniaProperty.Register<BoundsObserverBehavior, Rect>(nameof(Bounds), defaultBindingMode: BindingMode.OneWay);

    /// <summary>
    /// 
    /// </summary>
	public static readonly StyledProperty<double> WidthProperty =
		AvaloniaProperty.Register<BoundsObserverBehavior, double>(nameof(Width), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// 
    /// </summary>
	public static readonly StyledProperty<double> HeightProperty =
		AvaloniaProperty.Register<BoundsObserverBehavior, double>(nameof(Height), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// 
    /// </summary>
	public Rect Bounds
	{
		get => GetValue(BoundsProperty);
		set => SetValue(BoundsProperty, value);
	}

    /// <summary>
    /// 
    /// </summary>
	public double Width
	{
		get => GetValue(WidthProperty);
		set => SetValue(WidthProperty, value);
	}

    /// <summary>
    /// 
    /// </summary>
	public double Height
	{
		get => GetValue(HeightProperty);
		set => SetValue(HeightProperty, value);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
	protected override void OnAttached(CompositeDisposable disposables)
	{
		if (AssociatedObject is not null)
		{
			disposables.Add(this.GetObservable(BoundsProperty)
				.Subscribe(new AnonymousObserver<Rect>(bounds =>
                {
                    Width = bounds.Width;
                    Height = bounds.Height;
                })));
		}
	}
}
