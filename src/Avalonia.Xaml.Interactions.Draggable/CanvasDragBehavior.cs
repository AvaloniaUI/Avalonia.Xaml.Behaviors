using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Draggable;

/// <summary>
/// 
/// </summary>
public class CanvasDragBehavior : Behavior<Control>
{
    private bool _enableDrag;
    private Point _start;
    private Control? _parent;
    private Control? _draggedContainer;
    private Control? _adorner;
    private bool _captured;
    private Control? _rootControl;
    private IEnumerable<Visual>? _dragHandles;

    /// <summary>
    /// Determines if a control may be dragged. True by default.
    /// </summary>
    public static readonly AttachedProperty<bool> IsDraggableProperty = AvaloniaProperty.RegisterAttached<CanvasDragBehavior, Interactive, bool>("IsDraggable", true, false, BindingMode.TwoWay);

    /// <summary>
    /// Sets the value of the IsDraggable attached property for a control.
    /// </summary>
    /// <param name="element">The control.</param>
    /// <param name="isDraggableValue">The value to be set.</param>
    public static void SetIsDraggable(AvaloniaObject element, bool isDraggableValue)
    {
        element.SetValue(IsDraggableProperty, isDraggableValue);
    }

    /// <summary>
    /// Gets the value of the IsDraggable attached property for a control.
    /// </summary>
    /// <param name="element">The control.</param>
    public static bool GetIsDraggable(AvaloniaObject element)
    {
        return element.GetValue(IsDraggableProperty);
    }


    /// <summary>
    /// Determines if a control is a draghandle for a draggable ancestor.
    /// If at least one control has this property set to true, the draggable ancestor can only be moved via the specified drafhandles.
    /// False by default.
    /// </summary>
    public static readonly AttachedProperty<bool> IsDragHandleProperty = AvaloniaProperty.RegisterAttached<CanvasDragBehavior, Interactive, bool>("IsDragHandle", false, false, BindingMode.TwoWay);

    /// <summary>
    /// Sets the value of the IsDragHandle attached property for a control.
    /// </summary>
    /// <param name="element">The control.</param>
    /// <param name="isDragHandleValue">The value to be set.</param>
    public static void SetIsDragHandle(AvaloniaObject element, bool isDragHandleValue)
    {
        element.SetValue(IsDragHandleProperty, isDragHandleValue);
    }

    /// <summary>
    /// Gets the value of the IsDragHandle attached property for a control.
    /// <param name="element">The control.</param>
    /// </summary>
    public static bool GetIsDragHandle(AvaloniaObject element)
    {
        return element.GetValue(IsDragHandleProperty);
    }


    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is not null)
        {
            AssociatedObject.Loaded += OnLoaded;
        }
    }

    private void OnLoaded(object? sender, RoutedEventArgs e)
    {
        if (AssociatedObject is not null)
        {
            _rootControl = AssociatedObject is ContentPresenter presenter ? presenter.Child : AssociatedObject;

            _dragHandles = AssociatedObject.GetVisualDescendants().Where(d => d.GetValue(IsDragHandleProperty));

            if (_dragHandles.Any())
            {
                foreach (Control dragHandle in _dragHandles)
                {
                    dragHandle.AddHandler(InputElement.PointerReleasedEvent, Released, RoutingStrategies.Tunnel);
                    dragHandle.AddHandler(InputElement.PointerPressedEvent, Pressed, RoutingStrategies.Tunnel);
                    dragHandle.AddHandler(InputElement.PointerMovedEvent, Moved, RoutingStrategies.Tunnel);
                    dragHandle.AddHandler(InputElement.PointerCaptureLostEvent, CaptureLost, RoutingStrategies.Tunnel);
                }
            }
            else
            {
                AssociatedObject.AddHandler(InputElement.PointerReleasedEvent, Released, RoutingStrategies.Tunnel);
                AssociatedObject.AddHandler(InputElement.PointerPressedEvent, Pressed, RoutingStrategies.Tunnel);
                AssociatedObject.AddHandler(InputElement.PointerMovedEvent, Moved, RoutingStrategies.Tunnel);
                AssociatedObject.AddHandler(InputElement.PointerCaptureLostEvent, CaptureLost, RoutingStrategies.Tunnel);
            }
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is not null)
        {
            if (_dragHandles.Any())
            {
                foreach (Control dragHandle in _dragHandles)
                {
                    dragHandle.RemoveHandler(InputElement.PointerReleasedEvent, Released);
                    dragHandle.RemoveHandler(InputElement.PointerPressedEvent, Pressed);
                    dragHandle.RemoveHandler(InputElement.PointerMovedEvent, Moved);
                    dragHandle.RemoveHandler(InputElement.PointerCaptureLostEvent, CaptureLost);
                }
            }
            else
            {
                AssociatedObject.RemoveHandler(InputElement.PointerReleasedEvent, Released);
                AssociatedObject.RemoveHandler(InputElement.PointerPressedEvent, Pressed);
                AssociatedObject.RemoveHandler(InputElement.PointerMovedEvent, Moved);
                AssociatedObject.RemoveHandler(InputElement.PointerCaptureLostEvent, CaptureLost);
            }
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
            && AssociatedObject?.Parent is Control parent
            && _rootControl?.GetValue(IsDraggableProperty) == true
            )
        {
            _enableDrag = true;
            _start = e.GetPosition(parent);
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
            var deltaX = position.X - _start.X;
            var deltaY = position.Y - _start.Y;
            _start = position;
            var left = Canvas.GetLeft(_draggedContainer);
            var top = Canvas.GetTop(_draggedContainer);
            Canvas.SetLeft(_draggedContainer, left + deltaX);
            Canvas.SetTop(_draggedContainer, top + deltaY);
        }
    }

    private void Released()
    {
        if (_enableDrag)
        {
            if (_parent is not null && _draggedContainer is not null)
            {
                // RemoveAdorner(_draggedContainer);
            }

            if (_draggedContainer is not null)
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
