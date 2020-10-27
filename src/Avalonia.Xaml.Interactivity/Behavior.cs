using System;
using System.Diagnostics;
using System.Globalization;

namespace Avalonia.Xaml.Interactivity
{
    /// <summary>
    /// A base class for behaviors, implementing the basic plumbing of <see cref="IBehavior"/>.
    /// </summary>
    public abstract class Behavior : AvaloniaObject, IBehavior
    {
        /// <summary>
        /// Gets the <see cref="IAvaloniaObject"/> to which the behavior is attached.
        /// </summary>
        public IAvaloniaObject? AssociatedObject { get; private set; }

        /// <summary>
        /// Attaches the behavior to the specified <see cref="IAvaloniaObject"/>.
        /// </summary>
        /// <param name="associatedObject">The <see cref="IAvaloniaObject"/> to which to attach.</param>
        /// <exception cref="ArgumentNullException"><paramref name="associatedObject"/> is null.</exception>
        public void Attach(IAvaloniaObject? associatedObject)
        {
            if (associatedObject == AssociatedObject)
            {
                return;
            }

            if (AssociatedObject is { })
            {
                throw new InvalidOperationException(string.Format(
                    CultureInfo.CurrentCulture,
                    "An instance of a behavior cannot be attached to more than one object at a time.",
                    associatedObject,
                    AssociatedObject));
            }

            Debug.Assert(associatedObject is { }, "Cannot attach the behavior to a null object.");
            AssociatedObject = associatedObject ?? throw new ArgumentNullException(nameof(associatedObject));

            OnAttached();
        }

        /// <summary>
        /// Detaches the behaviors from the <see cref="AssociatedObject"/>.
        /// </summary>
        public void Detach()
        {
            OnDetaching();
            AssociatedObject = null;
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="AssociatedObject"/>.
        /// </summary>
        /// <remarks>
        /// Override this to hook up functionality to the <see cref="AssociatedObject"/>
        /// </remarks>
        protected virtual void OnAttached()
        {
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="AssociatedObject"/>.
        /// </summary>
        /// <remarks>
        /// Override this to unhook functionality from the <see cref="AssociatedObject"/>
        /// </remarks>
        protected virtual void OnDetaching()
        {
        }
    }
}
