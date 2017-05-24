// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Core
{
    /// <summary>
    /// A behavior that allows controls to be moved around the canvas using RenderTransform of AssociatedObject.
    /// </summary>
    public sealed class DragPositionBehavior : Behavior<Control>
    {
        private IControl _parent = null;
        private Point _previous;

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PointerPressed += AssociatedObject_PointerPressed;
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PointerPressed -= AssociatedObject_PointerPressed;
            _parent = null;
        }

        private void AssociatedObject_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            _parent = AssociatedObject.Parent;

            if (!(AssociatedObject.RenderTransform is TranslateTransform))
            {
                AssociatedObject.RenderTransform = new TranslateTransform();
            }

            _previous = e.GetPosition(_parent);
            _parent.PointerMoved += Parent_PointerMoved;
            _parent.PointerReleased += Parent_PointerReleased;
        }

        private void Parent_PointerMoved(object sender, PointerEventArgs args)
        {
            var pos = args.GetPosition(_parent);
            var tr = (TranslateTransform)AssociatedObject.RenderTransform;
            tr.X += pos.X - _previous.X;
            tr.Y += pos.Y - _previous.Y;
            _previous = pos;
        }

        private void Parent_PointerReleased(object sender, PointerReleasedEventArgs e)
        {
            _parent.PointerMoved -= Parent_PointerMoved;
            _parent.PointerReleased -= Parent_PointerReleased;
            _parent = null;
        }
    }
}
