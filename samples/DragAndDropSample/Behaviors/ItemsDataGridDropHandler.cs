using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;
using DragAndDropSample.ViewModels;

namespace DragAndDropSample.Behaviors;

public class ItemsDataGridDropHandler : DropHandlerBase
{
    private bool Validate<T>(DataGrid dg, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute) where T : ItemViewModel
    {
        if (sourceContext is not T sourceItem
            || targetContext is not MainWindowViewModel vm
            || dg.GetVisualAt(e.GetPosition(dg)) is not Control targetControl
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
        if (e.Source is Control && sender is DataGrid dg)
        {
            return Validate<ItemViewModel>(dg, e, sourceContext, targetContext, false);
        }
        return false;
    }

    public override bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
    {
        if (e.Source is Control && sender is DataGrid dg)
        {
            return Validate<ItemViewModel>(dg, e, sourceContext, targetContext, true);
        }
        return false;
    }
}
