// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using System.Reflection;
using Avalonia.Xaml.Interactivity;
using Avalonia.Controls;

namespace Avalonia.Xaml.Interactions.Core
{
    /// <summary>
    /// A behavior that listens for a specified event on its source and executes its actions when that event is fired.
    /// </summary>
    public sealed class EventTriggerBehavior : Trigger
    {
        private const string EventNameDefaultValue = "Loaded";

        static EventTriggerBehavior()
        {
            EventNameProperty.Changed.Subscribe(e =>
            {
                EventTriggerBehavior behavior = (EventTriggerBehavior)e.Sender;
                if (behavior.AssociatedObject == null || behavior.resolvedSource == null)
                {
                    return;
                }

                string oldEventName = (string)e.OldValue;
                string newEventName = (string)e.NewValue;

                behavior.UnregisterEvent(oldEventName);
                behavior.RegisterEvent(newEventName);
            });

            SourceObjectProperty.Changed.Subscribe(e =>
            {
                EventTriggerBehavior behavior = (EventTriggerBehavior)e.Sender;
                behavior.SetResolvedSource(behavior.ComputeResolvedSource());
            });
        }

        /// <summary>
        /// Identifies the <seealso cref="EventName"/> avalonia property.
        /// </summary>
        public static readonly AvaloniaProperty<string> EventNameProperty =
            AvaloniaProperty.Register<EventTriggerBehavior, string>(nameof(EventName), EventNameDefaultValue);

        /// <summary>
        /// Identifies the <seealso cref="SourceObject"/> avalonia property.
        /// </summary>
        public static readonly AvaloniaProperty<object> SourceObjectProperty =
            AvaloniaProperty.Register<EventTriggerBehavior, object>(nameof(SourceObject), AvaloniaProperty.UnsetValue);

        private object resolvedSource;
        private Delegate eventHandler;
        private bool isLoadedEventRegistered;

        /// <summary>
        /// Gets or sets the name of the event to listen for. This is a avalonia property.
        /// </summary>
        public string EventName
        {
            get => GetValue(EventNameProperty);
            set => SetValue(EventNameProperty, value);
        }

        /// <summary>
        /// Gets or sets the source object from which this behavior listens for events.
        /// If <seealso cref="SourceObject"/> is not set, the source will default to <seealso cref="Behavior.AssociatedObject"/>. This is a avalonia property.
        /// </summary>
        public object SourceObject
        {
            get => GetValue(SourceObjectProperty);
            set => SetValue(SourceObjectProperty, value);
        }

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            SetResolvedSource(ComputeResolvedSource());
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            SetResolvedSource(null);
        }

        private void SetResolvedSource(object newSource)
        {
            if (AssociatedObject == null || resolvedSource == newSource)
            {
                return;
            }

            if (resolvedSource != null)
            {
                UnregisterEvent(EventName);
            }

            resolvedSource = newSource;

            if (resolvedSource != null)
            {
                RegisterEvent(EventName);
            }
        }

        private object ComputeResolvedSource()
        {
            // If the SourceObject property is set at all, we want to use it. It is possible that it is data
            // bound and bindings haven't been evaluated yet. Plus, this makes the API more predictable.
            if (GetValue(SourceObjectProperty) != AvaloniaProperty.UnsetValue)
            {
                return SourceObject;
            }

            return AssociatedObject;
        }

        private void RegisterEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            if (eventName != EventNameDefaultValue)
            {
                Type sourceObjectType = resolvedSource.GetType();
                EventInfo info = sourceObjectType.GetRuntimeEvent(EventName);
                if (info == null)
                {
                    throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture,
                        "Cannot find an event named {0} on type {1}.",
                        EventName,
                        sourceObjectType.Name));
                }

                MethodInfo methodInfo = typeof(EventTriggerBehavior).GetTypeInfo().GetDeclaredMethod("OnEvent");
                eventHandler = methodInfo.CreateDelegate(info.EventHandlerType, this);
                info.AddEventHandler(resolvedSource, eventHandler);
            }
            else if (!isLoadedEventRegistered)
            {
                if (resolvedSource is Control element && !IsElementLoaded(element))
                {
                    isLoadedEventRegistered = true;
                    element.AttachedToVisualTree += OnEvent;
                }
            }
        }

        private void UnregisterEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                return;
            }

            if (eventName != EventNameDefaultValue)
            {
                if (eventHandler == null)
                {
                    return;
                }

                EventInfo info = resolvedSource.GetType().GetRuntimeEvent(eventName);
                info.RemoveEventHandler(resolvedSource, eventHandler);
                eventHandler = null;
            }
            else if (isLoadedEventRegistered)
            {
                isLoadedEventRegistered = false;
                Control element = (Control)resolvedSource;
                element.AttachedToVisualTree -= OnEvent;
            }
        }

        private void OnEvent(object sender, object eventArgs)
        {
            Interaction.ExecuteActions(resolvedSource, Actions, eventArgs);
        }

        internal static bool IsElementLoaded(Control element)
        {
            if (element == null)
            {
                return false;
            }

            return (element.Parent != null);
        }
    }
}
