using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom
{
    /// <summary>
    /// A behavior that allows controls to be moved around the canvas using RenderTransform of <see cref="Behavior.AssociatedObject"/>.
    /// </summary>
    public class DragPositionBehavior : Behavior<Control>
    {
        private IControl? _parent;
        private Point _previous;

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is { })
            {
                AssociatedObject.PointerPressed += AssociatedObject_PointerPressed; 
            }
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject is { })
            {
                AssociatedObject.PointerPressed -= AssociatedObject_PointerPressed; 
            }
            _parent = null;
        }

        private void AssociatedObject_PointerPressed(object? sender, PointerPressedEventArgs e)
        {
            if (AssociatedObject is { })
            {
                _parent = AssociatedObject.Parent;

                if (AssociatedObject.RenderTransform is not TranslateTransform)
                {
                    AssociatedObject.RenderTransform = new TranslateTransform();
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
            if (AssociatedObject is { })
            {
                var pos = args.GetPosition(_parent);
                if (AssociatedObject.RenderTransform is TranslateTransform tr)
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
}
