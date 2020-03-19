// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

        private void ActionCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            NotifyCollectionChangedAction collectionChange = eventArgs.Action;

            if (collectionChange == NotifyCollectionChangedAction.Reset)
            {
                foreach (var item in this)
                {
                    VerifyType(item);
                }
            }
            else if (collectionChange == NotifyCollectionChangedAction.Add || collectionChange == NotifyCollectionChangedAction.Replace)
            {
                var changedItem = (IAvaloniaObject)eventArgs.NewItems[0];
                VerifyType(changedItem);
            }
        }

        private static void VerifyType(IAvaloniaObject item)
        {
            if (!(item is IAction))
            {
                throw new InvalidOperationException("Only IAction types are supported in an ActionCollection.");
            }
        }
    }
}
