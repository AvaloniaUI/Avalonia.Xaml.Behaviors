using System;
using System.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media.Transformation;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Draggable;

/// <summary>
/// 
/// </summary>
public class ItemDragBehavior : Behavior<IControl>
{
    private bool _enableDrag;
    private bool _dragStarted;
    private Point _start;
    private int _draggedIndex;
    private int _targetIndex;
    private ItemsControl? _itemsControl;
    private IControl? _draggedContainer;

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<Orientation> OrientationProperty = 
        AvaloniaProperty.Register<ItemDragBehavior, Orientation>(nameof(Orientation));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<double> HorizontalDragThresholdProperty = 
        AvaloniaProperty.Register<ItemDragBehavior, double>(nameof(HorizontalDragThreshold), 3);

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<double> VerticalDragThresholdProperty =
        AvaloniaProperty.Register<ItemDragBehavior, double>(nameof(VerticalDragThreshold), 3);

    /// <summary>
    /// 
    /// </summary>
    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public double HorizontalDragThreshold
    {
        get => GetValue(HorizontalDragThresholdProperty);
        set => SetValue(HorizontalDragThresholdProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public double VerticalDragThreshold
    {
        get => GetValue(VerticalDragThresholdProperty);
        set => SetValue(VerticalDragThresholdProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is { })
        {
            AssociatedObject.AddHandler(InputElement.PointerReleasedEvent, PointerReleased, RoutingStrategies.Tunnel);
            AssociatedObject.AddHandler(InputElement.PointerPressedEvent, PointerPressed, RoutingStrategies.Tunnel);
            AssociatedObject.AddHandler(InputElement.PointerMovedEvent, PointerMoved, RoutingStrategies.Tunnel);
            AssociatedObject.AddHandler(InputElement.PointerCaptureLostEvent, PointerCaptureLost, RoutingStrategies.Tunnel);
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is { })
        {
            AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, PointerReleased);
            AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, PointerPressed);
            AssociatedObject.RemoveHandler(InputElement.PointerMovedEvent, PointerMoved);
            AssociatedObject.RemoveHandler(InputElement.PointerCaptureLostEvent, PointerCaptureLost);
        }
    }

    private void PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var properties = e.GetCurrentPoint(AssociatedObject).Properties;
        if (properties.IsLeftButtonPressed 
            && AssociatedObject?.Parent is ItemsControl itemsControl)
        {
            _enableDrag = true;
            _dragStarted = false;
            _start = e.GetPosition(AssociatedObject.Parent);
            _draggedIndex = -1;
            _targetIndex = -1;
            _itemsControl = itemsControl;
            _draggedContainer = AssociatedObject;

            if (_draggedContainer is { })
            {
                SetDraggingPseudoClasses(_draggedContainer, true);
            }

            AddTransforms(_itemsControl);

            e.Pointer.Capture(AssociatedObject);
        }
    }

    private void PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (Equals(e.Pointer.Captured, AssociatedObject))
        {
            if (e.InitialPressMouseButton == MouseButton.Left)
            {
                Released();
            }

            e.Pointer.Capture(null); 
        }
    }

    private void PointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
    {
        Released();
    }

    private void Released()
    {
        if (!_enableDrag)
        {
            return;
        }

        RemoveTransforms(_itemsControl);

        if (_itemsControl is { })
        {
            foreach (var container in _itemsControl.ItemContainerGenerator.Containers)
            {
                SetDraggingPseudoClasses(container.ContainerControl, true);
            }
        }

        if (_dragStarted)
        {
            if (_draggedIndex >= 0 && _targetIndex >= 0 && _draggedIndex != _targetIndex)
            {
                MoveDraggedItem(_itemsControl, _draggedIndex, _targetIndex);
            }
        }

        if (_itemsControl is { })
        {
            foreach (var container in _itemsControl.ItemContainerGenerator.Containers)
            {
                SetDraggingPseudoClasses(container.ContainerControl, false);
            }
        }

        if (_draggedContainer is { })
        {
            SetDraggingPseudoClasses(_draggedContainer, false);
        }

        _draggedIndex = -1;
        _targetIndex = -1;
        _enableDrag = false;
        _dragStarted = false;
        _itemsControl = null;

        _draggedContainer = null;
    }

    private void AddTransforms(ItemsControl? itemsControl)
    {
        if (itemsControl?.Items is null)
        {
            return;
        }

        var i = 0;

        foreach (var _ in itemsControl.Items)
        {
            var container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i);
            if (container is not null)
            {
                SetTranslateTransform(container, 0, 0);
            }
  
            i++;
        }  
    }

    private void RemoveTransforms(ItemsControl? itemsControl)
    {
        if (itemsControl?.Items is null)
        {
            return;
        }

        var i = 0;

        foreach (var _ in itemsControl.Items)
        {
            var container = itemsControl.ItemContainerGenerator.ContainerFromIndex(i);
            if (container is not null)
            {
                SetTranslateTransform(container, 0, 0);
            }
  
            i++;
        }  
    }

    private void MoveDraggedItem(ItemsControl? itemsControl, int draggedIndex, int targetIndex)
    {
        if (itemsControl?.Items is not IList items)
        {
            return;
        }

        var draggedItem = items[draggedIndex];
        items.RemoveAt(draggedIndex);
        items.Insert(targetIndex, draggedItem);

        if (itemsControl is SelectingItemsControl selectingItemsControl)
        {
            selectingItemsControl.SelectedIndex = targetIndex;
        }
    }

    private void PointerMoved(object? sender, PointerEventArgs e)
    {
        var properties = e.GetCurrentPoint(AssociatedObject).Properties;
        if (Equals(e.Pointer.Captured, AssociatedObject)
            && properties.IsLeftButtonPressed)
        {
            if (_itemsControl?.Items is null || _draggedContainer?.RenderTransform is null || !_enableDrag)
            {
                return;
            }

            var orientation = Orientation;
            var position = e.GetPosition(_itemsControl);
            var delta = orientation == Orientation.Horizontal ? position.X - _start.X : position.Y - _start.Y;

            if (!_dragStarted)
            {
                var diff = _start - position;
                var horizontalDragThreshold = HorizontalDragThreshold;
                var verticalDragThreshold = VerticalDragThreshold;

                if (orientation == Orientation.Horizontal)
                {
                    if (Math.Abs(diff.X) > horizontalDragThreshold)
                    {
                        _dragStarted = true;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (Math.Abs(diff.Y) > verticalDragThreshold)
                    {
                        _dragStarted = true;
                    }
                    else
                    {
                        return;
                    }
                }
            }

            if (orientation == Orientation.Horizontal)
            {
                SetTranslateTransform(_draggedContainer, delta, 0);
            }
            else
            {
                SetTranslateTransform(_draggedContainer, 0, delta);
            }

            _draggedIndex = _itemsControl.ItemContainerGenerator.IndexFromContainer(_draggedContainer);
            _targetIndex = -1;

            var draggedBounds = _draggedContainer.Bounds;

            var draggedStart = orientation == Orientation.Horizontal ? draggedBounds.X : draggedBounds.Y;

            var draggedDeltaStart = orientation == Orientation.Horizontal
                ? draggedBounds.X + delta
                : draggedBounds.Y + delta;

            var draggedDeltaEnd = orientation == Orientation.Horizontal
                ? draggedBounds.X + delta + draggedBounds.Width
                : draggedBounds.Y + delta + draggedBounds.Height;

            var i = 0;

            foreach (var _ in _itemsControl.Items)
            {
                var targetContainer = _itemsControl.ItemContainerGenerator.ContainerFromIndex(i);
                if (targetContainer?.RenderTransform is null || ReferenceEquals(targetContainer, _draggedContainer))
                {
                    i++;
                    continue;
                }

                var targetBounds = targetContainer.Bounds;

                var targetStart = orientation == Orientation.Horizontal ? targetBounds.X : targetBounds.Y;

                var targetMid = orientation == Orientation.Horizontal
                    ? targetBounds.X + targetBounds.Width / 2
                    : targetBounds.Y + targetBounds.Height / 2;

                var targetIndex = _itemsControl.ItemContainerGenerator.IndexFromContainer(targetContainer);

                if (targetStart > draggedStart && draggedDeltaEnd >= targetMid)
                {
                    if (orientation == Orientation.Horizontal)
                    {
                        SetTranslateTransform(targetContainer, -draggedBounds.Width, 0);
                    }
                    else
                    {
                        SetTranslateTransform(targetContainer, 0, -draggedBounds.Height);
                    }

                    _targetIndex = _targetIndex == -1 ? targetIndex :
                        targetIndex > _targetIndex ? targetIndex : _targetIndex;
                }
                else if (targetStart < draggedStart && draggedDeltaStart <= targetMid)
                {
                    if (orientation == Orientation.Horizontal)
                    {
                        SetTranslateTransform(targetContainer, draggedBounds.Width, 0);
                    }
                    else
                    {
                        SetTranslateTransform(targetContainer, 0, draggedBounds.Height);
                    }

                    _targetIndex = _targetIndex == -1 ? targetIndex :
                        targetIndex < _targetIndex ? targetIndex : _targetIndex;
                }
                else
                {
                    if (orientation == Orientation.Horizontal)
                    {
                        SetTranslateTransform(targetContainer, 0, 0);
                    }
                    else
                    {
                        SetTranslateTransform(targetContainer, 0, 0);
                    }
                }

                i++;
            }
        }
    }

    private void SetDraggingPseudoClasses(IControl control, bool isDragging)
    {
        if (isDragging)
        {
            ((IPseudoClasses)control.Classes).Add(":dragging");
        }
        else
        {
            ((IPseudoClasses)control.Classes).Remove(":dragging");
        }
    }

    private void SetTranslateTransform(IControl control, double x, double y)
    {
        var transformBuilder = new TransformOperations.Builder(1);
        transformBuilder.AppendTranslate(x, y);
        control.RenderTransform = transformBuilder.Build();
    }
}
