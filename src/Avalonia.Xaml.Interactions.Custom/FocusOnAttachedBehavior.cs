using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Threading;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class FocusOnAttachedBehavior : AttachedToVisualTreeBehavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
	public static readonly StyledProperty<bool> IsEnabledProperty =
		AvaloniaProperty.Register<FocusOnAttachedBehavior, bool>(nameof(IsEnabled), true);

    /// <summary>
    /// 
    /// </summary>
	public bool IsEnabled
	{
		get => GetValue(IsEnabledProperty);
		set => SetValue(IsEnabledProperty, value);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
	protected override void OnAttachedToVisualTree(CompositeDisposable disposables)
	{
		if (IsEnabled)
		{
			Dispatcher.UIThread.Post(() => AssociatedObject?.Focus());
		}
	}
}
