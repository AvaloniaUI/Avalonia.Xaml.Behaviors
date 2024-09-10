using System;
using System.Reactive.Disposables;
using Avalonia.Controls;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ScrollToItemBehavior : AttachedToVisualTreeBehavior<ItemsControl>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<IObservable<object>?> ItemProperty =
        AvaloniaProperty.Register<ScrollToItemBehavior, IObservable<object>?>(nameof(Item));

    /// <summary>
    /// 
    /// </summary>
    public IObservable<object>? Item
    {
        get => GetValue(ItemProperty);
        set => SetValue(ItemProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposables)
    {
        var disposable = Item?.Subscribe(item =>
            {
                AssociatedObject?.ScrollIntoView(item);
            });

        if (disposable is not null)
        {
            disposables.Add(disposable);
        }
    }
}
