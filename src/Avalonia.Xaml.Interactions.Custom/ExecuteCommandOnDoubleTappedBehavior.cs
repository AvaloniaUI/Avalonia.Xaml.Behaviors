using System.Reactive.Disposables;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ExecuteCommandOnDoubleTappedBehavior : DisposingBehavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
	public static readonly StyledProperty<ICommand?> CommandProperty =
		AvaloniaProperty.Register<ExecuteCommandOnDoubleTappedBehavior, ICommand?>(nameof(Command));

    /// <summary>
    /// 
    /// </summary>
	public ICommand? Command
	{
		get => GetValue(CommandProperty);
		set => SetValue(CommandProperty, value);
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
	protected override void OnAttached(CompositeDisposable disposables)
	{
        var disposable = Gestures.DoubleTappedEvent.AddClassHandler<InputElement>(
				(x, _) =>
				{
					if (Equals(x, AssociatedObject))
					{
						if (Command is { } cmd && cmd.CanExecute(default))
						{
							cmd.Execute(default);
						}
					}
				},
				RoutingStrategies.Tunnel | RoutingStrategies.Bubble);
        disposables.Add(disposable);
	}
}
