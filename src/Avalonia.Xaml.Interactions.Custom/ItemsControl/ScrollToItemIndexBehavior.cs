using System;
using System.Reactive.Disposables;
using Avalonia.Controls;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class ScrollToItemIndexBehavior : AttachedToVisualTreeBehavior<ItemsControl>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<IObservable<int>?> ItemIndexProperty =
        AvaloniaProperty.Register<ScrollToItemIndexBehavior, IObservable<int>?>(nameof(ItemIndex));

    /// <summary>
    /// 
    /// </summary>
    public IObservable<int>? ItemIndex
    {
        get => GetValue(ItemIndexProperty);
        set => SetValue(ItemIndexProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposables"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposables)
    {
        var disposable = ItemIndex?.Subscribe(index =>
        {
            AssociatedObject?.ScrollIntoView(index);
        });

        if (disposable is not null)
        {
            disposables.Add(disposable);
        }
    }
}
