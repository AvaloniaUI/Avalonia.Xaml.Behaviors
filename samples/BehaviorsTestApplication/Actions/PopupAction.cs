// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;
using System;

namespace BehaviorsTestApplication.Actions
{
    public class PopupAction : AvaloniaObject, IAction
    {
        public static readonly AvaloniaProperty<Control> ChildProperty =
            AvaloniaProperty.Register<PopupAction, Control>(nameof(Child));

        [Content]
        public Control Child
        {
            get { return this.GetValue(ChildProperty); }
            set { this.SetValue(ChildProperty, value); }
        }

        private Popup _popup = null;

        public object Execute(object sender, object parameter)
        {
            if (_popup == null)
            {
                _popup = new Popup()
                {
                    PlacementMode = PlacementMode.Pointer,
                    PlacementTarget = sender as Control,
                    StaysOpen = false
                };

                var control = sender as IControl;
                if (control != null)
                {
                    BindToDataContext(control, _popup);
                }
            }
            _popup.Child = Child;
            _popup.Open();
            return null;
        }

        private static void BindToDataContext(IControl source, IControl target)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (target == null)
                throw new ArgumentNullException(nameof(target));

            var data = source.GetObservable(Control.DataContextProperty);
            if (data != null)
            {
                target.Bind(Control.DataContextProperty, data);
            }
        }
    }
}
