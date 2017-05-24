// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Core
{
    /// <summary>
    /// A behavior that that displays cursor position on PointerMoved event for the AssociatedObject using TargetTextBlock.Text property.
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
            get { return (TextBlock)this.GetValue(TargetTextBlockProperty); }
            set { this.SetValue(TargetTextBlockProperty, value); }
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            this.AssociatedObject.PointerMoved += AssociatedObject_PointerMoved;
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            this.AssociatedObject.PointerMoved -= AssociatedObject_PointerMoved;
        }

        private void AssociatedObject_PointerMoved(object sender, Avalonia.Input.PointerEventArgs e)
        {
            if (TargetTextBlock != null)
            {
                TargetTextBlock.Text = e.GetPosition(this.AssociatedObject).ToString();
            }
        }
    }
}
