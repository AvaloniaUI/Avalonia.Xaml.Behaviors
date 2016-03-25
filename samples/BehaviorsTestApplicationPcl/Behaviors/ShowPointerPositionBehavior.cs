using Perspex;
using Perspex.Controls;
using Perspex.Xaml.Interactivity;

namespace BehaviorsTestApplication.Behaviors
{
    public sealed class ShowPointerPositionBehavior : Behavior<Control>
    {
        public static readonly PerspexProperty TargetTextBlockProperty =
            PerspexProperty.Register<ShowPointerPositionBehavior, TextBlock>(nameof(TargetTextBlock));

        public TextBlock TargetTextBlock
        {
            get { return (TextBlock)this.GetValue(TargetTextBlockProperty); }
            set { this.SetValue(TargetTextBlockProperty, value); }
        }

        private void AssociatedObject_PointerMoved(object sender, Perspex.Input.PointerEventArgs e)
        {
            if (TargetTextBlock != null)
            {
                TargetTextBlock.Text = e.GetPosition(this.AssociatedObject).ToString();
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PointerMoved += AssociatedObject_PointerMoved;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PointerMoved -= AssociatedObject_PointerMoved;
        }
    }
}
