using System.Collections.Generic;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.DragAndDrop;

/// <summary>
/// 
/// </summary>
public abstract class DropHandlerBase : IDropHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="items"></param>
    /// <param name="sourceIndex"></param>
    /// <param name="targetIndex"></param>
    /// <typeparam name="T"></typeparam>
    protected void MoveItem<T>(IList<T> items, int sourceIndex, int targetIndex)
    {
        if (sourceIndex < targetIndex)
        {
            var item = items[sourceIndex];
            items.RemoveAt(sourceIndex);
            items.Insert(targetIndex, item);
        }
        else
        {
            var removeIndex = sourceIndex + 1;
            if (items.Count + 1 > removeIndex)
            {
                var item = items[sourceIndex];
                items.RemoveAt(removeIndex - 1);
                items.Insert(targetIndex, item);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceItems"></param>
    /// <param name="targetItems"></param>
    /// <param name="sourceIndex"></param>
    /// <param name="targetIndex"></param>
    /// <typeparam name="T"></typeparam>
    protected void MoveItem<T>(IList<T> sourceItems, IList<T> targetItems, int sourceIndex, int targetIndex)
    {
        var item = sourceItems[sourceIndex];
        sourceItems.RemoveAt(sourceIndex);
        targetItems.Insert(targetIndex, item);
    }
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="items"></param>
    /// <param name="sourceIndex"></param>
    /// <param name="targetIndex"></param>
    /// <typeparam name="T"></typeparam>
    protected void SwapItem<T>(IList<T> items, int sourceIndex, int targetIndex)
    {
        var item1 = items[sourceIndex];
        var item2 = items[targetIndex];
        items[targetIndex] = item1;
        items[sourceIndex] = item2;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceItems"></param>
    /// <param name="targetItems"></param>
    /// <param name="sourceIndex"></param>
    /// <param name="targetIndex"></param>
    /// <typeparam name="T"></typeparam>
    protected void SwapItem<T>(IList<T> sourceItems, IList<T> targetItems, int sourceIndex, int targetIndex)
    {
        var item1 = sourceItems[sourceIndex];
        var item2 = targetItems[targetIndex];
        targetItems[targetIndex] = item1;
        sourceItems[sourceIndex] = item2;
    }
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="items"></param>
    /// <param name="item"></param>
    /// <param name="index"></param>
    /// <typeparam name="T"></typeparam>
    protected void InsertItem<T>(IList<T> items, T item, int index)
    {
        items.Insert(index, item);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="sourceContext"></param>
    /// <param name="targetContext"></param>
    public virtual void Enter(object? sender, DragEventArgs e, object? sourceContext, object? targetContext)
    {
        if (Validate(sender, e, sourceContext, targetContext, null) == false)
        {
            e.DragEffects = DragDropEffects.None;
            e.Handled = true;
        }
        else
        {
            e.DragEffects |= DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link;
            e.Handled = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="sourceContext"></param>
    /// <param name="targetContext"></param>
    public virtual void Over(object? sender, DragEventArgs e, object? sourceContext, object? targetContext)
    {
        if (Validate(sender, e, sourceContext, targetContext, null) == false)
        {
            e.DragEffects = DragDropEffects.None;
            e.Handled = true;
        }
        else
        {
            e.DragEffects |= DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link;
            e.Handled = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="sourceContext"></param>
    /// <param name="targetContext"></param>
    public virtual void Drop(object? sender, DragEventArgs e, object? sourceContext, object? targetContext)
    {
        if (Execute(sender, e, sourceContext, targetContext, null) == false)
        {
            e.DragEffects = DragDropEffects.None;
            e.Handled = true;
        }
        else
        {
            e.DragEffects |= DragDropEffects.Copy | DragDropEffects.Move | DragDropEffects.Link;
            e.Handled = true;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public virtual void Leave(object? sender, RoutedEventArgs e)
    {
        Cancel(sender, e);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="sourceContext"></param>
    /// <param name="targetContext"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public virtual bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
    {
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="sourceContext"></param>
    /// <param name="targetContext"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    public virtual bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
    {
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public virtual void Cancel(object? sender, RoutedEventArgs e)
    {
    }
}