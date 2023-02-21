using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// A behavior that allows controls to be moved around the canvas using RenderTransform of <see cref="Behavior.AssociatedObject"/>.
/// </summary>
public sealed class DragControlBehavior : Behavior<Control>
{
    /// <summary>
    /// Identifies the <seealso cref="TargetControl"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<Control?> TargetControlProperty =
        AvaloniaProperty.Register<DragControlBehavior, Control?>(nameof(TargetControl));

    private Control? _parent;
    private Point _previous;

    /// <summary>
    /// Gets or sets the target control to be moved around instead of <see cref="IBehavior.AssociatedObject"/>. This is a avalonia property.
    /// </summary>
    [ResolveByName]
    public Control? TargetControl
    {
        get => GetValue(TargetControlProperty);
        set => SetValue(TargetControlProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        var source = AssociatedObject;
        if (source is { })
        {
            source.PointerPressed += Source_PointerPressed;
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        var source = AssociatedObject;
        if (source is { })
        {
            source.PointerPressed -= Source_PointerPressed;
        }

        _parent = null;
    }

    private void Source_PointerPressed(object? sender, PointerPressedEventArgs e)
    {
        var target = TargetControl ?? AssociatedObject;
        if (target is { })
        {
            _parent = target.Parent as Control;

            if (!(target.RenderTransform is TranslateTransform))
            {
                target.RenderTransform = new TranslateTransform();
            }

            _previous = e.GetPosition(_parent);
            if (_parent != null)
            {
                _parent.PointerMoved += Parent_PointerMoved;
                _parent.PointerReleased += Parent_PointerReleased;
            }
        }
    }

    private void Parent_PointerMoved(object? sender, PointerEventArgs args)
    {
        var target = TargetControl ?? AssociatedObject;
        if (target is { })
        {
            var pos = args.GetPosition(_parent);
            if (target.RenderTransform is TranslateTransform tr)
            {
                tr.X += pos.X - _previous.X;
                tr.Y += pos.Y - _previous.Y;
            }
            _previous = pos;
        }
    }

    private void Parent_PointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (_parent is { })
        {
            _parent.PointerMoved -= Parent_PointerMoved;
            _parent.PointerReleased -= Parent_PointerReleased;
            _parent = null;
        }
    }
}
