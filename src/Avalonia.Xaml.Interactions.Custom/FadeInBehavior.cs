using System;
using Avalonia.Animation;
using Avalonia.Styling;
using System.Reactive.Disposables;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class FadeInBehavior : AttachedToVisualTreeBehavior<Visual>
{
    /// <summary>
    /// 
    /// </summary>
	public static readonly StyledProperty<TimeSpan> InitialDelayProperty =
		AvaloniaProperty.Register<FadeInBehavior, TimeSpan>(nameof(InitialDelay), TimeSpan.FromMilliseconds(500));

    /// <summary>
    /// 
    /// </summary>
	public static readonly StyledProperty<TimeSpan> DurationProperty =
		AvaloniaProperty.Register<FadeInBehavior, TimeSpan>(nameof(Duration), TimeSpan.FromMilliseconds(250));

    /// <summary>
    /// 
    /// </summary>
	public TimeSpan InitialDelay
	{
		get => GetValue(InitialDelayProperty);
		set => SetValue(InitialDelayProperty, value);
	}

    /// <summary>
    /// 
    /// </summary>
	public TimeSpan Duration
	{
		get => GetValue(DurationProperty);
		set => SetValue(DurationProperty, value);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposable"></param>
	protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
	{
		if (AssociatedObject is null)
		{
			return;
		}

		var totalDuration = InitialDelay + Duration;

		var animation = new Animation.Animation
		{
			Duration = totalDuration,
			Children =
			{
				new KeyFrame
				{
					KeyTime = TimeSpan.Zero,
					Setters =
					{
						new Setter(Visual.OpacityProperty, 0d),
					}
				},
				new KeyFrame
				{
					KeyTime = InitialDelay,
					Setters =
					{
						new Setter(Visual.OpacityProperty, 0d),
					}
				},
				new KeyFrame
				{
					KeyTime = Duration,
					Setters =
					{
						new Setter(Visual.OpacityProperty, 1d),
					}
				}
			}
		};
		animation.RunAsync(AssociatedObject);
	}
}
