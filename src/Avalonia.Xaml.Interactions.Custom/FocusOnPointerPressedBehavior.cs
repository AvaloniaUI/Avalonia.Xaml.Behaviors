// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom
{
    /// <summary>
    /// Focuses the <see cref="Behavior{T}.AssociatedObject"/> on <see cref="InputElement.PointerPressed"/> event.
    /// </summary>
    public sealed class FocusOnPointerPressedBehavior : Behavior<Control>
    {
        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PointerPressed += PointerPressed;
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PointerPressed -= PointerPressed;
        }

        private void PointerPressed(object sender, PointerPressedEventArgs e)
        {
            AssociatedObject.Focus();
        }
    }
}
