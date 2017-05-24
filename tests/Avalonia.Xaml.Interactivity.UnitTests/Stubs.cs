// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System.Collections.Generic;

namespace Avalonia.Xaml.Interactivity.UnitTests
{
    public class StubBehavior : AvaloniaObject, IBehavior
    {
        public int AttachCount
        {
            get;
            private set;
        }

        public int DetachCount
        {
            get;
            private set;
        }

        public ActionCollection Actions
        {
            get;
            private set;
        }

        public StubBehavior()
        {
            Actions = new ActionCollection();
        }

        public AvaloniaObject AssociatedObject
        {
            get;
            private set;
        }

        public void Attach(AvaloniaObject AvaloniaObject)
        {
            AssociatedObject = AvaloniaObject;
            AttachCount++;
        }

        public void Detach()
        {
            AssociatedObject = null;
            DetachCount++;
        }

        public IEnumerable<object> Execute(object sender, object parameter)
        {
            return Interaction.ExecuteActions(sender, Actions, parameter);
        }
    }

    public class StubAction : AvaloniaObject, IAction
    {
        private readonly object returnValue;

        public StubAction()
        {
            returnValue = null;
        }

        public StubAction(object returnValue)
        {
            this.returnValue = returnValue;
        }

        public object Sender
        {
            get;
            private set;
        }

        public object Parameter
        {
            get;
            private set;
        }

        public int ExecuteCount
        {
            get;
            private set;
        }

        public object Execute(object sender, object parameter)
        {
            ExecuteCount++;
            Sender = sender;
            Parameter = parameter;
            return returnValue;
        }
    }
}
