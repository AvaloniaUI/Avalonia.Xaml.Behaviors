using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom
{
    /// <summary>
    /// A behavior that displays cursor position on <see cref="InputElement.PointerMoved"/> event for the <see cref="Behavior{T}.AssociatedObject"/> using <see cref="TextBlock.Text"/> property.
    /// </summary>
    public sealed class ShowPointerPositionBehavior : Behavior<Control>
    {
        /// <summary>
        /// Identifies the <seealso cref="TargetTextBlockProperty"/> avalonia property.
        /// </summary>
        public static readonly AvaloniaProperty TargetTextBlockProperty =
            AvaloniaProperty.Register<ShowPointerPositionBehavior, TextBlock>(nameof(TargetTextBlock));

        /// <summary>
        /// Gets or sets the target TextBlock object in which this behavior displays cursor position on PointerMoved event.
        /// </summary>
        public TextBlock TargetTextBlock
        {
            get => (TextBlock)GetValue(TargetTextBlockProperty);
            set => SetValue(TargetTextBlockProperty, value);
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.PointerMoved += AssociatedObject_PointerMoved; 
            }
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
            {
                AssociatedObject.PointerMoved -= AssociatedObject_PointerMoved; 
            }
        }

        private void AssociatedObject_PointerMoved(object sender, PointerEventArgs e)
        {
            if (TargetTextBlock != null)
            {
                TargetTextBlock.Text = e.GetPosition(AssociatedObject).ToString();
            }
        }
    }
}
