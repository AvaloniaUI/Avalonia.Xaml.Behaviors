// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom
{
    /// <summary>
    /// Focuses the <see cref="Behavior.AssociatedObject"/> when attached to visual tree.
    /// </summary>
    public sealed class FocusOnAttachedToVisualTreeBehavior : Behavior<Control>
    {
        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
            {
                AssociatedObject.AttachedToVisualTree += AttachedToVisualTree; 
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
                AssociatedObject.AttachedToVisualTree -= AttachedToVisualTree; 
            }
        }

        private void AttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
        {
            AssociatedObject?.Focus();
        }
    }
}
