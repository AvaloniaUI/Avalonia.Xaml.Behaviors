// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Avalonia.Xaml.Interactivity
{
    /// <summary>
    /// A base class for behaviors, implementing the basic plumbing of ITrigger
    /// </summary>
    /// <typeparam name="T">The object type to attach to</typeparam>
    public abstract class Trigger<T> : Trigger where T : AvaloniaObject
    {
        /// <summary>
        /// Gets the object to which this behavior is attached.
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        public new T AssociatedObject => base.AssociatedObject as T;
        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the <see cref="Behavior.AssociatedObject"/>
        /// </remarks>
        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject == null)
            {
                string actualType = base.AssociatedObject.GetType().FullName;
                string expectedType = typeof(T).FullName;
                string message = string.Format("AssociatedObject is of type {0} but should be of type {1}.", actualType, expectedType);
                throw new InvalidOperationException(message);
            }
        }
    }
}
