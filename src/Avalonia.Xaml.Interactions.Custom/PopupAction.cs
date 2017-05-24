// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See licence.md file in the project root for full license information.

using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom
{
    /// <summary>
    /// An action that displays a Popup for the associated control when executed.
    /// </summary>
    /// <remarks>If the associated control is of type IControl than popup inherits control DataContext.</remarks>
    public sealed class PopupAction : AvaloniaObject, IAction
    {
        private Popup _popup = null;
    
        /// <summary>
        /// Identifies the <seealso cref="ChildProperty"/> avalonia property.
        /// </summary>
        public static readonly AvaloniaProperty<Control> ChildProperty =
            AvaloniaProperty.Register<PopupAction, Control>(nameof(Child));

        /// <summary>
        /// Gets or sets the popup Child control. This is a avalonia property.
        /// </summary>
        [Content]
        public Control Child
        {
            get { return this.GetValue(ChildProperty); }
            set { this.SetValue(ChildProperty, value); }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>Returns null after executed.</returns>
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
