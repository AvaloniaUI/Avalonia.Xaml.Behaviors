using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Input;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ExecuteCommandOnActivatedBehavior : DisposingBehavior<Control>
{
	public static readonly StyledProperty<ICommand?> CommandProperty =
		AvaloniaProperty.Register<ExecuteCommandOnActivatedBehavior, ICommand?>(nameof(Command));

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
		if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime lifetime)
		{
			var mainWindow = lifetime.MainWindow;

			var dispose = Observable
				.FromEventPattern(mainWindow, nameof(mainWindow.Activated))
				.Subscribe(new AnonymousObserver<EventPattern<object>>(_ =>
                {
                    if (Command is { } cmd && cmd.CanExecute(default))
                    {
                        cmd.Execute(default);
                    }
                }));
            disposables.Add(dispose);
		}
	}
}
