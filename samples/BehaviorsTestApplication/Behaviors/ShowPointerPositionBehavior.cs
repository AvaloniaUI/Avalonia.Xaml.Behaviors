// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace BehaviorsTestApplication.Behaviors
{
    public sealed class ShowPointerPositionBehavior : Behavior<Control>
    {
        public static readonly AvaloniaProperty TargetTextBlockProperty =
            AvaloniaProperty.Register<ShowPointerPositionBehavior, TextBlock>(nameof(TargetTextBlock));

        public TextBlock TargetTextBlock
        {
            get { return (TextBlock)this.GetValue(TargetTextBlockProperty); }
            set { this.SetValue(TargetTextBlockProperty, value); }
        }

        private void AssociatedObject_PointerMoved(object sender, Avalonia.Input.PointerEventArgs e)
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
