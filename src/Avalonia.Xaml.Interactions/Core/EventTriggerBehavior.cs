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
                if (behavior.AssociatedObject == null || behavior._resolvedSource == null)
                {
                    return;
                }

                var oldEventName = (string?)e.OldValue;
                var newEventName = (string?)e.NewValue;

                if (oldEventName != null)
                {
                    behavior.UnregisterEvent(oldEventName);
                }

                if (newEventName != null)
                {
                    behavior.RegisterEvent(newEventName);
                }
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
        public static readonly StyledProperty<string> EventNameProperty =
            AvaloniaProperty.Register<EventTriggerBehavior, string>(nameof(EventName), EventNameDefaultValue);

        /// <summary>
        /// Identifies the <seealso cref="SourceObject"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<object?> SourceObjectProperty =
            AvaloniaProperty.Register<EventTriggerBehavior, object?>(nameof(SourceObject), null);

        private object? _resolvedSource;
        private Delegate? _eventHandler;
        private bool _isLoadedEventRegistered;

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
        public object? SourceObject
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

        private void SetResolvedSource(object? newSource)
        {
            if (AssociatedObject == null || _resolvedSource == newSource)
            {
                return;
            }

            if (_resolvedSource != null)
            {
                UnregisterEvent(EventName);
            }

            _resolvedSource = newSource;

            if (_resolvedSource != null)
            {
                RegisterEvent(EventName);
            }
        }

        private object? ComputeResolvedSource()
        {
            // If the SourceObject property is set at all, we want to use it. It is possible that it is data
            // bound and bindings haven't been evaluated yet. Plus, this makes the API more predictable.
            if (GetValue(SourceObjectProperty) != null)
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
                if (_resolvedSource != null)
                {
                    Type sourceObjectType = _resolvedSource.GetType();
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
                    _eventHandler = methodInfo.CreateDelegate(info.EventHandlerType, this);
                    info.AddEventHandler(_resolvedSource, _eventHandler); 
                }
            }
            else if (!_isLoadedEventRegistered)
            {
                if (_resolvedSource is Control element && !IsElementLoaded(element))
                {
                    _isLoadedEventRegistered = true;
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
                if (_eventHandler == null)
                {
                    return;
                }

                if (_resolvedSource != null)
                {
                    EventInfo info = _resolvedSource.GetType().GetRuntimeEvent(eventName);
                    info.RemoveEventHandler(_resolvedSource, _eventHandler); 
                }
                _eventHandler = null;
            }
            else if (_isLoadedEventRegistered)
            {
                _isLoadedEventRegistered = false;
                if (_resolvedSource != null)
                {
                    Control element = (Control)_resolvedSource;
                    element.AttachedToVisualTree -= OnEvent; 
                }
            }
        }

        private void OnEvent(object sender, object eventArgs)
        {
            Interaction.ExecuteActions(_resolvedSource, Actions, eventArgs);
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
