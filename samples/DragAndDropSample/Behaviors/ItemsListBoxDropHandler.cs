using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;
using DragAndDropSample.ViewModels;

namespace DragAndDropSample.Behaviors
{
    public class ItemsListBoxDropHandler : DropHandlerBase
    {
        private bool Validate<T>(ListBox listBox, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute) where T : ItemViewModel
        {
            if (sourceContext is not T sourceItem
                || targetContext is not MainWindowViewModel vm
                || listBox.GetVisualAt(e.GetPosition(listBox)) is not IControl targetControl
                || targetControl.DataContext is not T targetItem)
            {
                return false;
            }

            var items = vm.Items;
            var sourceIndex = items.IndexOf(sourceItem);
            var targetIndex = items.IndexOf(targetItem);

            if (sourceIndex < 0 || targetIndex < 0)
            {
                return false;
            }

            switch (e.DragEffects)
            {
                case DragDropEffects.Copy:
                {
                    if (bExecute)
                    {
                        var clone = new ItemViewModel() { Title = sourceItem.Title + "_copy" };
                        InsertItem(items, clone, targetIndex + 1);
                    }
                    return true;
                }
                case DragDropEffects.Move:
                {
                    if (bExecute)
                    {
                        MoveItem(items, sourceIndex, targetIndex);
                    }
                    return true;
                }
                case DragDropEffects.Link:
                {
                    if (bExecute)
                    {
                        SwapItem(items, sourceIndex, targetIndex);
                    }
                    return true;
                }
                default:
                    return false;
            }
        }
        
        public override bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            if (e.Source is IControl && sender is ListBox listBox)
            {
                return Validate<ItemViewModel>(listBox, e, sourceContext, targetContext, false);
            }
            return false;
        }

        public override bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
        {
            if (e.Source is IControl && sender is ListBox listBox)
            {
                return Validate<ItemViewModel>(listBox, e, sourceContext, targetContext, true);
            }
            return false;
        }
    }
}
