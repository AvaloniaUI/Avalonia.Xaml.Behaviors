// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Core
{
    public sealed class DragPositionBehavior : Behavior<Control>
    {
        private IControl parent = null;
        private Point prevPoint;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PointerPressed += AssociatedObject_PointerPressed;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PointerPressed -= AssociatedObject_PointerPressed;
            parent = null;
        }

        private void AssociatedObject_PointerPressed(object sender, PointerPressedEventArgs e)
        {
            parent = AssociatedObject.Parent;

            if (!(AssociatedObject.RenderTransform is TranslateTransform))
            {
                AssociatedObject.RenderTransform = new TranslateTransform();
            }

            prevPoint = e.GetPosition(parent);
            parent.PointerMoved += Parent_PointerMoved;
            parent.PointerReleased += Parent_PointerReleased;
        }

        private void Parent_PointerMoved(object sender, PointerEventArgs args)
        {
            var pos = args.GetPosition(parent);
            var tr = (TranslateTransform)AssociatedObject.RenderTransform;
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
