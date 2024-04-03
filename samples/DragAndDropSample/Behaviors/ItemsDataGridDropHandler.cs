using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using DragAndDropSample.ViewModels;

namespace DragAndDropSample.Behaviors;

public sealed class ItemsDataGridDropHandler : BaseDataGridDropHandler<ItemViewModel>
{
    protected override ItemViewModel MakeCopy(ObservableCollection<ItemViewModel> parentCollection, ItemViewModel item) =>
        new() { Title = item.Title };

    protected override bool Validate(DataGrid dg, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute)
    {
        if (sourceContext is not ItemViewModel sourceItem
         || targetContext is not MainWindowViewModel vm
         || dg.GetVisualAt(e.GetPosition(dg)) is not Control targetControl
         || targetControl.DataContext is not ItemViewModel targetItem)
        {
            return false;
        }

        var items = vm.Items;
        return RunDropAction(dg, e, bExecute, sourceItem, targetItem, items);
    }
}