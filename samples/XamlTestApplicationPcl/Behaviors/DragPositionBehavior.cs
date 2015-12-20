using Perspex;
using Perspex.Controls;
using Perspex.Input;
using Perspex.Media;
using Perspex.Xaml.Interactivity;

namespace XamlTestApplication.Behaviors
{
    public class DragPositionBehavior : Behavior
    {
        private Control parent = null;
        private Point prevPoint;

        protected override void OnAttached()
        {
            base.OnAttached();

            var control = AssociatedObject as Control;
            if (control != null)
            {
                control.PointerPressed += AssociatedObject_PointerPressed;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            var control = AssociatedObject as Control;
            if (control != null)
            {
                control.PointerPressed -= AssociatedObject_PointerPressed;
            }

            parent = null;
        }

        private void AssociatedObject_PointerPressed(object sender, PointerPressEventArgs e)
        {
            var control = AssociatedObject as Control;
            parent = (Control)control.Parent;

            if (!(control.RenderTransform is TranslateTransform))
            {
                control.RenderTransform = new TranslateTransform();
            }

            prevPoint = e.GetPosition(parent);
            parent.PointerMoved += Parent_PointerMoved;
            parent.PointerReleased += Parent_PointerReleased;
        }

        private void Parent_PointerMoved(object sender, PointerEventArgs args)
        {
            var control = AssociatedObject as Control;
            var pos = args.GetPosition(parent);
            var tr = (TranslateTransform)control.RenderTransform;
            tr.X += pos.X - prevPoint.X;
            tr.Y += pos.Y - prevPoint.Y;
            prevPoint = pos;
        }

        private void Parent_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            parent.PointerMoved -= Parent_PointerMoved;
            parent.PointerReleased -= Parent_PointerReleased;
            parent = null;
        }
    }
}
