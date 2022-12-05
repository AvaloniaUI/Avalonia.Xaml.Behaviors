using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactions.DragAndDrop;
using DragAndDropSample.ViewModels;

namespace DragAndDropSample.Behaviors;

public class NodesTreeViewDropHandler : DropHandlerBase
{
    private bool Validate<T>(TreeView treeView, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute) where T : NodeViewModel
    {
        if (sourceContext is not T sourceNode
            || targetContext is not MainWindowViewModel vm
            || treeView.GetVisualAt(e.GetPosition(treeView)) is not Control targetControl
            || targetControl.DataContext is not T targetNode)
        {
            return false;
        }

        var sourceParent = sourceNode.Parent;
        var targetParent = targetNode.Parent;
        var sourceNodes = sourceParent is { } ? sourceParent.Nodes : vm.Nodes;
        var targetNodes = targetParent is { } ? targetParent.Nodes : vm.Nodes;

        if (sourceNodes is { } && targetNodes is { })
        {
            var sourceIndex = sourceNodes.IndexOf(sourceNode);
            var targetIndex = targetNodes.IndexOf(targetNode);

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
                        var clone = new NodeViewModel() { Title = sourceNode.Title + "_copy" };
                        InsertItem(targetNodes, clone, targetIndex + 1);
                    }

                    return true;
                }
                case DragDropEffects.Move:
                {
                    if (bExecute)
                    {
                        if (sourceNodes == targetNodes)
                        {
                            MoveItem(sourceNodes, sourceIndex, targetIndex);
                        }
                        else
                        {
                            sourceNode.Parent = targetParent;

                            MoveItem(sourceNodes, targetNodes, sourceIndex, targetIndex);
                        }
                    }

                    return true;
                }
                case DragDropEffects.Link:
                {
                    if (bExecute)
                    {
                        if (sourceNodes == targetNodes)
                        {
                            SwapItem(sourceNodes, sourceIndex, targetIndex);
                        }
                        else
                        {
                            sourceNode.Parent = targetParent;
                            targetNode.Parent = sourceParent;

                            SwapItem(sourceNodes, targetNodes, sourceIndex, targetIndex);
                        }
                    }

                    return true;
                }
            }
        }

        return false;
    }
        
    public override bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
    {
        if (e.Source is Control && sender is TreeView treeView)
        {
            return Validate<NodeViewModel>(treeView, e, sourceContext, targetContext, false);
        }
        return false;
    }

    public override bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state)
    {
        if (e.Source is Control && sender is TreeView treeView)
        {
            return Validate<NodeViewModel>(treeView, e, sourceContext, targetContext, true);
        }
        return false;
    }
}
