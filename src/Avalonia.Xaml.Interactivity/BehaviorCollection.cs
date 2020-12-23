using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using Avalonia.Collections;

namespace Avalonia.Xaml.Interactivity
{
    /// <summary>
    /// Represents a collection of <see cref="IBehavior"/>'s with a shared <see cref="AssociatedObject"/>.
    /// </summary>
    public sealed class BehaviorCollection : AvaloniaList<IAvaloniaObject>
    {
        // After a VectorChanged event we need to compare the current state of the collection
        // with the old collection so that we can call Detach on all removed items.
        private readonly List<IBehavior> _oldCollection = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorCollection"/> class.
        /// </summary>
        public BehaviorCollection()
        {
            CollectionChanged += BehaviorCollection_CollectionChanged;
        }

        /// <summary>
        /// Gets the <see cref="IAvaloniaObject"/> to which the <see cref="BehaviorCollection"/> is attached.
        /// </summary>
        public IAvaloniaObject? AssociatedObject
        {
            get;
            private set;
        }

        /// <summary>
        /// Attaches the collection of behaviors to the specified <see cref="IAvaloniaObject"/>.
        /// </summary>
        /// <param name="associatedObject">The <see cref="IAvaloniaObject"/> to which to attach.</param>
        /// <exception cref="InvalidOperationException">The <see cref="BehaviorCollection"/> is already attached to a different <see cref="IAvaloniaObject"/>.</exception>
        public void Attach(IAvaloniaObject? associatedObject)
        {
            if (Equals(associatedObject, AssociatedObject))
            {
                return;
            }

            if (AssociatedObject is { })
            {
                throw new InvalidOperationException(
                    "An instance of a behavior cannot be attached to more than one object at a time.");
            }

            Debug.Assert(associatedObject is { }, "The previous checks should keep us from ever setting null here.");
            AssociatedObject = associatedObject;

            foreach (var item in this)
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
            foreach (var item in this)
            {
                if (item is IBehavior behaviorItem && behaviorItem.AssociatedObject is { })
                {
                    behaviorItem.Detach();
                }
            }

            AssociatedObject = null;
            _oldCollection.Clear();
        }

        private void BehaviorCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            if (eventArgs.Action == NotifyCollectionChangedAction.Reset)
            {
                foreach (var behavior in _oldCollection)
                {
                    if (behavior.AssociatedObject is { })
                    {
                        behavior.Detach();
                    }
                }

                _oldCollection.Clear();

                foreach (var newItem in this)
                {
                    _oldCollection.Add(VerifiedAttach(newItem));
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
                        var eventIndex = eventArgs.NewStartingIndex;
                        var changedItem = eventArgs.NewItems?[0] as IAvaloniaObject;
                        _oldCollection.Insert(eventIndex, VerifiedAttach(changedItem));
                    }
                    break;

                case NotifyCollectionChangedAction.Replace:
                    {
                        var eventIndex = eventArgs.OldStartingIndex;
                        eventIndex = eventIndex == -1 ? 0 : eventIndex;

                        var changedItem = eventArgs.NewItems?[0] as IAvaloniaObject;

                        var oldItem = _oldCollection[eventIndex];
                        if (oldItem.AssociatedObject is { })
                        {
                            oldItem.Detach();
                        }

                        _oldCollection[eventIndex] = VerifiedAttach(changedItem);
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    {
                        var eventIndex = eventArgs.OldStartingIndex;

                        var oldItem = _oldCollection[eventIndex];
                        if (oldItem.AssociatedObject is { })
                        {
                            oldItem.Detach();
                        }

                        _oldCollection.RemoveAt(eventIndex);
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

        private IBehavior VerifiedAttach(IAvaloniaObject? item)
        {
            if (!(item is IBehavior behavior))
            {
                throw new InvalidOperationException(
                    $"Only {nameof(IBehavior)} types are supported in a {nameof(BehaviorCollection)}.");
            }

            if (_oldCollection.Contains(behavior))
            {
                throw new InvalidOperationException(
                    $"Cannot add an instance of a behavior to a {nameof(BehaviorCollection)} more than once.");
            }

            if (AssociatedObject is { })
            {
                behavior.Attach(AssociatedObject);
            }

            return behavior;
        }

        [Conditional("DEBUG")]
        private void VerifyOldCollectionIntegrity()
        {
            bool isValid = Count == _oldCollection.Count;
            if (isValid)
            {
                for (int i = 0; i < Count; i++)
                {
                    if (!Equals(this[i], _oldCollection[i]))
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
