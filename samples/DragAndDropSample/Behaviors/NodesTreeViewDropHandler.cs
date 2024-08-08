using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.VisualTree;
using DragAndDropSample.ViewModels;

namespace DragAndDropSample.Behaviors;

public class NodesTreeViewDropHandler : BaseTreeViewDropHandler
{
    protected override (bool Valid, bool WillSourceItemBeMovedToDifferentParent) Validate(TreeView tv, DragEventArgs e, object? sourceContext, object? targetContext, bool bExecute)
    {
        if (sourceContext is not NodeViewModel sourceNode
            || targetContext is not MainWindowViewModel vm
            || tv.GetVisualAt(e.GetPosition(tv)) is not Control targetControl
            || targetControl.DataContext is not NodeViewModel targetNode
            || sourceNode == targetNode
            || sourceNode.Parent == targetNode
            || targetNode.IsDescendantOf(sourceNode) // block moving parent to inside child
            || vm.HasMultipleTreeNodesSelected)
        {
            // moving multiple items is disabled because 
            // when an item is clicked to be dragged (whilst pressing Ctrl),
            // it becomes unselected and won't be considered for movement.
            // TODO: find how to fix that.
            return (false, false);
        }

        var sourceParent = sourceNode.Parent;
        var targetParent = targetNode.Parent;
        var sourceNodes = sourceParent is not null ? sourceParent.Nodes : vm.Nodes;
        var targetNodes = targetParent is not null ? targetParent.Nodes : vm.Nodes;
        bool areSourceNodesDifferentThanTargetNodes = sourceNodes != targetNodes;

        if (sourceNodes is not null && targetNodes is not null)
        {
            var sourceIndex = sourceNodes.IndexOf(sourceNode);
            var targetIndex = targetNodes.IndexOf(targetNode);

            if (sourceIndex < 0 || targetIndex < 0)
            {
                return (false, false);
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

                    return (true, areSourceNodesDifferentThanTargetNodes);
                }
                case DragDropEffects.Move:
                {
                    if (bExecute)
                    {
                        if (sourceNodes == targetNodes)
                        {
                            if (sourceIndex < targetIndex)
                            {
                                sourceNodes.RemoveAt(sourceIndex);
                                sourceNodes.Insert(targetIndex, sourceNode);
                            }
                            else
                            {
                                int removeIndex = sourceIndex + 1;
                                if (sourceNodes.Count + 1 > removeIndex)
                                {
                                    sourceNodes.RemoveAt(removeIndex - 1);
                                    sourceNodes.Insert(targetIndex, sourceNode);
                                }
                            }
                        }
                        else
                        {
                            sourceNode.Parent = targetParent;
                            sourceNodes.RemoveAt(sourceIndex);
                            targetNodes.Add(sourceNode); // always adding to the end
                        }
                    }

                    return (true, areSourceNodesDifferentThanTargetNodes);
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

                    return (true, areSourceNodesDifferentThanTargetNodes);
                }
            }
        }

        return (false, false);
    }
}
