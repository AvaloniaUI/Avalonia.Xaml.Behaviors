// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom
{
    /// <summary>
    /// Releases mouse device when executed.
    /// </summary>
    /// <remarks>
    /// The <see cref="Behavior.AssociatedObject"/> must implement <see cref="IControl"/> interface.
    /// </remarks>
    public sealed class ReleaseMouseDeviceAction : AvaloniaObject, IAction
    {
        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>Returns null after executed.</returns>
        public object Execute(object sender, object parameter)
        {
            ((sender as IControl).VisualRoot as IInputRoot)?.MouseDevice.Capture(null);
            return null;
        }
    }
}
