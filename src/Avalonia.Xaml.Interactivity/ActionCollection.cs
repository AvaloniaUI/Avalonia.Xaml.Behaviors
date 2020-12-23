using System;
using System.Collections.Specialized;
using Avalonia.Collections;

namespace Avalonia.Xaml.Interactivity
{
    /// <summary>
    /// Represents a collection of <see cref="IAction"/>'s.
    /// </summary>
    public sealed class ActionCollection : AvaloniaList<IAvaloniaObject>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCollection"/> class.
        /// </summary>
        public ActionCollection()
        {
            CollectionChanged += ActionCollection_CollectionChanged;
        }

        private void ActionCollection_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            var collectionChangedAction = eventArgs.Action;

            if (collectionChangedAction == NotifyCollectionChangedAction.Reset)
            {
                foreach (var item in this)
                {
                    VerifyType(item);
                }
            }
            else if (collectionChangedAction == NotifyCollectionChangedAction.Add || collectionChangedAction == NotifyCollectionChangedAction.Replace)
            {
                var changedItem = eventArgs.NewItems?[0] as IAvaloniaObject;
                VerifyType(changedItem);
            }
        }

        private static void VerifyType(IAvaloniaObject? item)
        {
            if (!(item is IAction))
            {
                throw new InvalidOperationException("Only IAction types are supported in an ActionCollection.");
            }
        }
    }
}
