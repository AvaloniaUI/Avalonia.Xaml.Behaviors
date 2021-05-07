using System.Collections.Generic;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.DragAndDrop
{
    public abstract class DropHandlerBase : IDropHandler
    {
        protected void MoveItem<T>(IList<T> items, int sourceIndex, int targetIndex)
        {
            if (sourceIndex < targetIndex)
            {
                var item = items[sourceIndex];
                items.Insert(targetIndex + 1, item);
                items.RemoveAt(sourceIndex);
            }
            else
            {
                int removeIndex = sourceIndex + 1;
                if (items.Count + 1 > removeIndex)
                {
                    var item = items[sourceIndex];
                    items.Insert(targetIndex, item);
                    items.RemoveAt(removeIndex);
                }
            }
        }

        protected void SwapItem<T>(IList<T> items, int sourceIndex, int targetIndex)
        {
            var item1 = items[sourceIndex];
            var item2 = items[targetIndex];
            items[targetIndex] = item1;
            items[sourceIndex] = item2;
        }
        
        protected void InsertItem<T>(IList<T> items, T item, int index)
        {
            items.Insert(index, item);
        }

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

        public virtual void Leave(object? sender, RoutedEventArgs e)
        {
            Cancel(sender, e);
        }

        public virtual bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            return false;
        }

        public virtual bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            return false;
        }

        public virtual void Cancel(object? sender, RoutedEventArgs e)
        {
        }
    }
}
