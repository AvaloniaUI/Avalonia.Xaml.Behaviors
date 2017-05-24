// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom
{
    /// <summary>
    /// Toggles IsExpanded property of the associated TreeViewItem control on DoubleTapped event.
    /// </summary>
    public sealed class ToggleIsExpandedOnDoubleTappedBehavior : Behavior<Control>
    {
        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.DoubleTapped += DoubleTapped;
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.DoubleTapped -= DoubleTapped;
        }

        private void DoubleTapped(object sender, RoutedEventArgs args)
        {
            var treeViewItem = AssociatedObject.Parent as TreeViewItem;
            if (treeViewItem != null)
            {
                treeViewItem.IsExpanded = !treeViewItem.IsExpanded;
            }
        }
    }
}
