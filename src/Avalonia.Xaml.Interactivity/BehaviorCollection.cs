// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using Avalonia.Collections;

namespace Avalonia.Xaml.Interactivity
{
    /// <summary>
    /// Represents a collection of IBehaviors with a shared <see cref="AssociatedObject"/>.
    /// </summary>
    public sealed class BehaviorCollection : AvaloniaList<AvaloniaObject>
    {
        // After a VectorChanged event we need to compare the current state of the collection
        // with the old collection so that we can call Detach on all removed items.
        private readonly List<IBehavior> oldCollection = new List<IBehavior>();

        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorCollection"/> class.
        /// </summary>
        public BehaviorCollection()
        {
            CollectionChanged += BehaviorCollection_CollectionChanged;
        }

        /// <summary>
        /// Gets the <see cref="AvaloniaObject"/> to which the <see cref="BehaviorCollection"/> is attached.
        /// </summary>
        public AvaloniaObject AssociatedObject
        {
            get;
            private set;
        }

        /// <summary>
        /// Attaches the collection of behaviors to the specified <see cref="AvaloniaObject"/>.
        /// </summary>
        /// <param name="associatedObject">The <see cref="AvaloniaObject"/> to which to attach.</param>
        /// <exception cref="InvalidOperationException">The <see cref="BehaviorCollection"/> is already attached to a different <see cref="AvaloniaObject"/>.</exception>
        public void Attach(AvaloniaObject associatedObject)
        {
            if (associatedObject == AssociatedObject)
            {
                return;
            }

            if (AssociatedObject != null)
            {
                throw new InvalidOperationException("An instance of a behavior cannot be attached to more than one object at a time.");
            }

            Debug.Assert(associatedObject != null, "The previous checks should keep us from ever setting null here.");
            AssociatedObject = associatedObject;

            foreach (AvaloniaObject item in this)
            {
                IBehavior behavior = (IBehavior)item;
                behavior.Attach(AssociatedObject);
            }
        }

        /// <summary>
        /// Detaches the collection of behaviors from the <see cref="BehaviorCollection.AssociatedObject"/>.
        /// </summary>
        public void Detach()
        {
            foreach (AvaloniaObject item in this)
            {
                IBehavior behaviorItem = (IBehavior)item;
                if (behaviorItem.AssociatedObject != null)
                {
                    behaviorItem.Detach();
                }
            }

            AssociatedObject = null;
            oldCollection.Clear();
        }

        private void BehaviorCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            if (eventArgs.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (IBehavior behavior in oldCollection)
                {
                    if (behavior.AssociatedObject != null)
                    {
                        behavior.Detach();
                    }
                }

                oldCollection.Clear();

                foreach (AvaloniaObject newItem in this)
                {
                    oldCollection.Add(VerifiedAttach(newItem));
                }
#if DEBUG
                VerifyOldCollectionIntegrity();
#endif
                return;
            }

            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    {
                        int eventIndex = eventArgs.NewStartingIndex;
                        AvaloniaObject changedItem = (AvaloniaObject)eventArgs.NewItems[0];
                        oldCollection.Insert(eventIndex, VerifiedAttach(changedItem));
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    {
                        int eventIndex = eventArgs.OldStartingIndex;
                        eventIndex = eventIndex == -1 ? 0 : eventIndex;

                        AvaloniaObject changedItem = (AvaloniaObject)eventArgs.NewItems[0];

                        IBehavior oldItem = oldCollection[eventIndex];
                        if (oldItem.AssociatedObject != null)
                        {
                            oldItem.Detach();
                        }

                        oldCollection[eventIndex] = VerifiedAttach(changedItem);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        int eventIndex = eventArgs.OldStartingIndex;
                        AvaloniaObject changedItem = (AvaloniaObject)eventArgs.OldItems[0];

                        IBehavior oldItem = oldCollection[eventIndex];
                        if (oldItem.AssociatedObject != null)
                        {
                            oldItem.Detach();
                        }

                        oldCollection.RemoveAt(eventIndex);
                    }
                    break;

                default:
                    Debug.Assert(false, "Unsupported collection operation attempted.");
                    break;
            }
#if DEBUG
            VerifyOldCollectionIntegrity();
#endif
        }

        private IBehavior VerifiedAttach(AvaloniaObject item)
        {
            IBehavior behavior = item as IBehavior;
            if (behavior == null)
            {
                throw new InvalidOperationException("Only IBehavior types are supported in a BehaviorCollection.");
            }

            if (oldCollection.Contains(behavior))
            {
                throw new InvalidOperationException("Cannot add an instance of a behavior to a BehaviorCollection more than once.");
            }

            if (AssociatedObject != null)
            {
                behavior.Attach(AssociatedObject);
            }

            return behavior;
        }

        [Conditional("DEBUG")]
        private void VerifyOldCollectionIntegrity()
        {
            bool isValid = (Count == oldCollection.Count);
            if (isValid)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (this[i] != oldCollection[i])
                    {
                        isValid = false;
                        break;
                    }
                }
            }

            Debug.Assert(isValid, "Referential integrity of the collection has been compromised.");
        }
    }
}
