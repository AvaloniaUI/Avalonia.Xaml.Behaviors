using System;
using System.Reactive.Disposables;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// 
/// </summary>
public class FocusSelectedItemBehavior : AttachedToVisualTreeBehavior<ItemsControl>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="disposable"></param>
    protected override void OnAttachedToVisualTree(CompositeDisposable disposable)
    {
        var dispose = AssociatedObject?
            .GetObservable(SelectingItemsControl.SelectedItemProperty)
            .Subscribe(selectedItem =>
            {
                var item = selectedItem;
                if (item is not null)
                {
                    Dispatcher.UIThread.Post(() =>
                    {
                        var container = AssociatedObject.ContainerFromItem(item);
                        if (container is not null)
                        {
                            container.Focus();
                        }
                    });
                }
            });

        if (dispose is not null)
        {
            disposable.Add(dispose);
        }
    }
}
