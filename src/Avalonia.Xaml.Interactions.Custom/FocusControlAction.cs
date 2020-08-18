using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom
{
    /// <summary>
    /// Focuses the associated control when executed.
    /// </summary>
    public sealed class FocusControlAction : AvaloniaObject, IAction
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>Returns null after executed.</returns>
        public object? Execute(object? sender, object? parameter)
        {
            (sender as Control)?.Focus();
            return null;
        }
    }
}
