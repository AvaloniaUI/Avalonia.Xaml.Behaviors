using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom
{
    /// <summary>
    /// A behavior that allows to select all <see cref="TextBox"/> text on got focus event.
    /// </summary>
    public class SelectAllOnGotFocusBehavior : Behavior<TextBox>
    {
        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject?.AddHandler(InputElement.GotFocusEvent, AssociatedObject_GotFocus, RoutingStrategies.Bubble);
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject?.RemoveHandler(InputElement.GotFocusEvent, AssociatedObject_GotFocus);
        }

        private void AssociatedObject_GotFocus(object? sender, GotFocusEventArgs e)
        {
            AssociatedObject?.SelectAll();
        }
    }
}
