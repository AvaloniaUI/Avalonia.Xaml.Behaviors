using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Draggable;

/// <summary>
/// 
/// </summary>
public class GridDragBehavior : Behavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> CopyColumnProperty = 
        AvaloniaProperty.Register<GridDragBehavior, bool>(nameof(CopyColumn), true);

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> CopyRowProperty = 
        AvaloniaProperty.Register<GridDragBehavior, bool>(nameof(CopyRow), true);

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> CopyColumnSpanProperty =
        AvaloniaProperty.Register<GridDragBehavior, bool>(nameof(CopyColumnSpan));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<bool> CopyRowSpanProperty = 
        AvaloniaProperty.Register<GridDragBehavior, bool>(nameof(CopyRowSpan));

    private bool _enableDrag;
    private Control? _parent;
    private Control? _draggedContainer;
    private Control? _adorner;
    private bool _captured;
        
    /// <summary>
    /// 
    /// </summary>
    public bool CopyColumn
    {
        get => GetValue(CopyColumnProperty);
        set => SetValue(CopyColumnProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public bool CopyRow
    {
        get => GetValue(CopyRowProperty);
        set => SetValue(CopyRowProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public bool CopyColumnSpan
    {
        get => GetValue(CopyColumnSpanProperty);
        set => SetValue(CopyColumnSpanProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public bool CopyRowSpan
    {
        get => GetValue(CopyRowSpanProperty);
        set => SetValue(CopyRowSpanProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is { })
        {
            AssociatedObject.AddHandler(InputElement.PointerReleasedEvent, Released, RoutingStrategies.Tunnel);
            AssociatedObject.AddHandler(InputElement.PointerPressedEvent, Pressed, RoutingStrategies.Tunnel);
            AssociatedObject.AddHandler(InputElement.PointerMovedEvent, Moved, RoutingStrategies.Tunnel);
            AssociatedObject.AddHandler(InputElement.PointerCaptureLostEvent, CaptureLost, RoutingStrategies.Tunnel);
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is { })
        {
            AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, Released);
            AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, Pressed);
            AssociatedObject.RemoveHandler(InputElement.PointerMovedEvent, Moved);
            AssociatedObject.RemoveHandler(InputElement.PointerCaptureLostEvent, CaptureLost);
        }
    }

    private void AddAdorner(Control control)
    {
        var layer = AdornerLayer.GetAdornerLayer(control);
        if (layer is null)
        {
            return;
        }

        _adorner = new SelectionAdorner()
        {
            [AdornerLayer.AdornedElementProperty] = control
        };

        ((ISetLogicalParent) _adorner).SetParent(control);
        layer.Children.Add(_adorner);
    }

    private void RemoveAdorner(Control control)
    {
        var layer = AdornerLayer.GetAdornerLayer(control);
        if (layer is null || _adorner is null)
        {
            return;
        }

        layer.Children.Remove(_adorner);
        ((ISetLogicalParent) _adorner).SetParent(null);
        _adorner = null;
    }

    private void Pressed(object? sender, PointerPressedEventArgs e)
    {
        var properties = e.GetCurrentPoint(AssociatedObject).Properties;
        if (properties.IsLeftButtonPressed
            && AssociatedObject?.Parent is Control parent)
        {

            _enableDrag = true;
            _parent = parent;
            _draggedContainer = AssociatedObject;

            SetDraggingPseudoClasses(_draggedContainer, true);

            // AddAdorner(_draggedContainer);

            _captured = true;
        }
    }

    private void Released(object? sender, PointerReleasedEventArgs e)
    {
        if (_captured)
        {
            if (e.InitialPressMouseButton == MouseButton.Left)
            {
                Released();
            }

            _captured = false;
        }
    }

    private void CaptureLost(object? sender, PointerCaptureLostEventArgs e)
    {
        Released();
        _captured = false;
    }

    private void Moved(object? sender, PointerEventArgs e)
    {
        var properties = e.GetCurrentPoint(AssociatedObject).Properties;
        if (_captured
            && properties.IsLeftButtonPressed)
        {
            if (_parent is null || _draggedContainer is null || !_enableDrag)
            {
                return;
            }

            var position = e.GetPosition(_parent);

            Control? target = null;


            if (_parent is Grid grid)
            {
                foreach (var child in grid.Children)
                {
                    if (Equals(child, _draggedContainer))
                    {
                        continue;
                    }

                    if (child.Bounds.Contains(position) && child is Control control)
                    {
                        target = control;
                    }
                }
            }
            else if (_parent is ItemsControl itemsControl)
            {
                foreach (var control in itemsControl.GetRealizedContainers())
                {
                    if (control is  null)
                    {
                        continue;
                    }

                    if (Equals(control, _draggedContainer))
                    {
                        continue;
                    }

                    if (control.Bounds.Contains(position))
                    {
                        target = control;
                    }
                }
            }
            else
            {
                return;
            }

            if (target is null)
            {
                return;
            }

            var copyColumn = CopyColumn;
            var copyRow = CopyRow;
            var copyColumnSpan = CopyColumnSpan;
            var copyRowSpan = CopyRowSpan;

            var sourceColumn = Grid.GetColumn(_draggedContainer);
            var sourceRow = Grid.GetRow(_draggedContainer);
            var sourceColumnSpan = Grid.GetColumnSpan(_draggedContainer);
            var sourceRowSpan = Grid.GetRowSpan(_draggedContainer);

            var targetColumn = Grid.GetColumn(target);
            var targetRow = Grid.GetRow(target);
            var targetColumnSpan = Grid.GetColumnSpan(target);
            var targetRowSpan = Grid.GetRowSpan(target);

            if (copyColumn)
            {
                Grid.SetColumn(_draggedContainer, targetColumn);
            }

            if (copyRow)
            {
                Grid.SetRow(_draggedContainer, targetRow);
            }

            if (copyColumnSpan)
            {
                Grid.SetColumnSpan(_draggedContainer, targetColumnSpan);
            }

            if (copyRowSpan)
            {
                Grid.SetRowSpan(_draggedContainer, targetRowSpan);
            }

            if (copyColumn)
            {
                Grid.SetColumn(target, sourceColumn);
            }

            if (copyRow)
            {
                Grid.SetRow(target, sourceRow);
            }

            if (copyColumnSpan)
            {
                Grid.SetColumnSpan(target, sourceColumnSpan);
            }

            if (copyRowSpan)
            {
                Grid.SetRowSpan(target, sourceRowSpan);
            }
        }
    }

    private void Released()
    {
        if (_enableDrag)
        {
            if (_parent is { } && _draggedContainer is { })
            {
                // RemoveAdorner(_draggedContainer);
            }

            if (_draggedContainer is { })
            {
                SetDraggingPseudoClasses(_draggedContainer, false);
            }

            _enableDrag = false;
            _parent = null;
            _draggedContainer = null;
        }
    }

    private void SetDraggingPseudoClasses(Control control, bool isDragging)
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
}
