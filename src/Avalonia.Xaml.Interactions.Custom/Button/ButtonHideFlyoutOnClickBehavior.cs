using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ButtonHideFlyoutOnClickBehavior : AttachedToVisualTreeBehavior<Button>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposables)
    {
		if (AssociatedObject is null)
		{
			return;
		}
        
		var flyoutPresenter = AssociatedObject.FindAncestorOfType<FlyoutPresenter>();
		if (flyoutPresenter?.Parent is not Popup popup)
		{
			return;
		}

		var disposable = Observable
			.FromEventPattern<RoutedEventArgs>(handler => AssociatedObject.Click += handler, handler => AssociatedObject.Click -= handler)
			.Do(_ =>
			{
				// Execute Command if any before closing. Otherwise, it won't execute because Close will destroy the associated object before Click can execute it.
				if (AssociatedObject.Command != null && AssociatedObject.IsEnabled)
				{
					AssociatedObject.Command.Execute(AssociatedObject.CommandParameter);
				}
				popup.Close();
			})
            .Subscribe();
        
        disposables.Add(disposable);
	}
}
