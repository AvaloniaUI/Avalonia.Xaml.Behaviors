using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Controls.Generators;
using Avalonia.Controls.Presenters;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Support for nth-child selector styling classes in item collections.
/// </summary>
/// <remarks>
/// Supported classes: nth-first-child, nth-last-child, nth-odd-child, nth-even-child
/// </remarks>
public class NthChildBehavior : Behavior<IControl>
{
    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is { })
        {
            Enable(true, AssociatedObject);
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is { })
        {
            Enable(false, AssociatedObject);
        }
    }

    private void Enable(bool value, IControl control)
    {
        var generator = control switch {
            ItemsPresenter ip => ip.ItemContainerGenerator,
            ItemsControl ic => ic.ItemContainerGenerator,
            _ => throw new NotSupportedException("Only ItemsPresenter and ItemsControl are supported at the moment")
        };

        if (value)
        {
            generator.Dematerialized += ItemContainerGeneratorOnDematerialized;
            generator.Materialized += ItemContainerGeneratorOnMaterialized;
            generator.Recycled += ItemContainerGeneratorOnMaterialized;
        }
        else
        {
            generator.Dematerialized -= ItemContainerGeneratorOnDematerialized;
            generator.Materialized -= ItemContainerGeneratorOnMaterialized;
            generator.Recycled -= ItemContainerGeneratorOnMaterialized;

        }
    }

    private void ItemContainerGeneratorOnMaterialized(object? sender, ItemContainerEventArgs e)
    {
        var count = GetOwnersItemsCount(sender);

        ItemContainerGeneratorOnEvent(false, count, e);
    }

    private void ItemContainerGeneratorOnDematerialized(object? sender, ItemContainerEventArgs e)
    {
        var count = GetOwnersItemsCount(sender);

        ItemContainerGeneratorOnEvent(true, count, e);
    }

    private void ItemContainerGeneratorOnEvent(bool dematerialized, int? count, ItemContainerEventArgs e)
    {
        foreach (var container in e.Containers)
        {
            var index = container.Index;
            var control = container.ContainerControl;
            var classes = control.Classes;
            classes.Set("nth-first-child", !dematerialized && index == 0);
            classes.Set("nth-last-child", !dematerialized && count.HasValue && index == count.Value - 1);
            classes.Set("nth-odd-child", !dematerialized && index % 2 != 0);
            classes.Set("nth-even-child", !dematerialized && index % 2 == 0);
        }
    }

    private int? GetOwnersItemsCount(object? sender)
    {
        if ((sender as ItemContainerGenerator)?.Owner is { } owner)
        {
            if (owner.IsSet(ItemsControl.ItemCountProperty))
            {
                return owner.GetValue(ItemsControl.ItemCountProperty) as int?;
            }
            else
            {
                var items = owner.GetValue(ItemsControl.ItemsProperty);
                return items switch
                {
                    IList list => list.Count,
                    IReadOnlyCollection<object> roc => roc.Count,
                    IEnumerable<object> enumerable => enumerable.Count(),
                    IEnumerable enumerableObj => enumerableObj.OfType<object>().Count(),
                    _ => null
                };
            }
        }

        return null;
    }
}
