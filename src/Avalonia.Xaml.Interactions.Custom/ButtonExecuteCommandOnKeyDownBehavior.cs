using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ButtonExecuteCommandOnKeyDownBehavior : AttachedToVisualTreeBehavior<Button>
{
    /// <summary>
    /// 
    /// </summary>
	public static readonly StyledProperty<Key?> KeyProperty =
		AvaloniaProperty.Register<ButtonExecuteCommandOnKeyDownBehavior, Key?>(nameof(Key));

    /// <summary>
    /// 
    /// </summary>
	public static readonly StyledProperty<bool> IsEnabledProperty =
		AvaloniaProperty.Register<ButtonExecuteCommandOnKeyDownBehavior, bool>(nameof(IsEnabled));

    /// <summary>
    /// 
    /// </summary>
	public Key? Key
	{
		get => GetValue(KeyProperty);
		set => SetValue(KeyProperty, value);
	}

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
		var button = AssociatedObject;
		if (button is null)
		{
			return;
		}

		if (button.GetVisualRoot() is InputElement inputRoot)
		{
            var disposable = inputRoot.AddDisposableHandler(InputElement.KeyDownEvent, RootDefaultKeyDown);
            disposables.Add(disposable);
		}
	}

	private void RootDefaultKeyDown(object? sender, KeyEventArgs e)
	{
		var button = AssociatedObject;
		if (button is null)
		{
			return;
		}

		if (Key is { } && e.Key == Key && button.IsVisible && button.IsEnabled && IsEnabled)
		{
			if (!e.Handled && button.Command?.CanExecute(button.CommandParameter) == true)
			{
				button.Command.Execute(button.CommandParameter);
				e.Handled = true;
			}
		}
	}
}
