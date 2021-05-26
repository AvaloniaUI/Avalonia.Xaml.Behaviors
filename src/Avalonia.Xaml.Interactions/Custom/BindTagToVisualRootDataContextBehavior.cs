using System;
using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom
{
    /// <summary>
    /// Binds AssociatedObject object Tag property to root visual DataContext.
    /// </summary>
    public sealed class BindTagToVisualRootDataContextBehavior : Behavior<Control>
    {
        private IDisposable? _disposable;

        /// <inheritdoc/>
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is { })
            {
                AssociatedObject.AttachedToVisualTree += AttachedToVisualTree; 
            }
        }

        /// <inheritdoc/>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject is { })
            {
                AssociatedObject.AttachedToVisualTree -= AttachedToVisualTree; 
            }
            _disposable?.Dispose();
        }

        private void AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            _disposable = BindDataContextToTag((IControl)AssociatedObject.GetVisualRoot(), AssociatedObject);
        }

        private static IDisposable? BindDataContextToTag(IControl source, IControl? target)
        {
            if (source is null)
                throw new ArgumentNullException(nameof(source));

            if (target is null)
                throw new ArgumentNullException(nameof(target));

            var data = source.GetObservable(StyledElement.DataContextProperty);
            return data is { } ? target.Bind(Control.TagProperty, data) : null;
        }
    }
}
