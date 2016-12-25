// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Avalonia.Xaml.Interactivity
{
    /// <summary>
    /// Interface implemented by all custom behaviors.
    /// </summary>
    public interface IBehavior
    {
        /// <summary>
        /// Gets the <see cref="AvaloniaObject"/> to which the <seealso cref="IBehavior"/> is attached.
        /// </summary>
        AvaloniaObject AssociatedObject { get; }

        /// <summary>
        /// Attaches to the specified object.
        /// </summary>
        /// <param name="associatedObject">The <see cref="AvaloniaObject"/> to which the <seealso cref="IBehavior"/> will be attached.</param>
        void Attach(AvaloniaObject associatedObject);

        /// <summary>
        /// Detaches this instance from its associated object.
        /// </summary>
        void Detach();
    }
}
