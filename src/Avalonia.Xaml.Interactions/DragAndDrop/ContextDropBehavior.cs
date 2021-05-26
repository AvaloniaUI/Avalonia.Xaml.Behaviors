﻿using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.DragAndDrop
{
    public sealed class ContextDropBehavior : Behavior<Control>
    {
        public static string DataFormat = nameof(Context);

        public static readonly StyledProperty<object?> ContextProperty =
            AvaloniaProperty.Register<ContextDropBehavior, object?>(nameof(Context));

        public static readonly StyledProperty<IDropHandler?> HandlerProperty =
            AvaloniaProperty.Register<ContextDropBehavior, IDropHandler?>(nameof(Handler));

        public object? Context
        {
            get => GetValue(ContextProperty);
            set => SetValue(ContextProperty, value);
        }

        public IDropHandler? Handler
        {
            get => GetValue(HandlerProperty);
            set => SetValue(HandlerProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is { })
            {
                DragDrop.SetAllowDrop(AssociatedObject, true);
            }
            AssociatedObject?.AddHandler(DragDrop.DragEnterEvent, DragEnter);
            AssociatedObject?.AddHandler(DragDrop.DragLeaveEvent, DragLeave);
            AssociatedObject?.AddHandler(DragDrop.DragOverEvent, DragOver);
            AssociatedObject?.AddHandler(DragDrop.DropEvent, Drop);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject is { })
            {
                DragDrop.SetAllowDrop(AssociatedObject, false);
            }
            AssociatedObject?.RemoveHandler(DragDrop.DragEnterEvent, DragEnter);
            AssociatedObject?.RemoveHandler(DragDrop.DragLeaveEvent, DragLeave);
            AssociatedObject?.RemoveHandler(DragDrop.DragOverEvent, DragOver);
            AssociatedObject?.RemoveHandler(DragDrop.DropEvent, Drop);
        }

        private void DragEnter(object? sender, DragEventArgs e)
        {
            var sourceContext = e.Data.Get(ContextDropBehavior.DataFormat);
            var targetContext = Context;
            Handler?.Enter(sender, e, sourceContext, targetContext);
        }

        private void DragLeave(object? sender, RoutedEventArgs e)
        {
            Handler?.Leave(sender, e);
        }

        private void DragOver(object? sender, DragEventArgs e)
        {
            var sourceContext = e.Data.Get(ContextDropBehavior.DataFormat);
            var targetContext = Context;
            Handler?.Over(sender, e, sourceContext, targetContext);
        }

        private void Drop(object? sender, DragEventArgs e)
        {
            var sourceContext = e.Data.Get(ContextDropBehavior.DataFormat);
            var targetContext = Context;
            Handler?.Drop(sender, e, sourceContext, targetContext);
        }
    }
}
