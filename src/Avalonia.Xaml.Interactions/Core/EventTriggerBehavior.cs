using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using Avalonia.Xaml.Interactivity;
using Avalonia.Controls;
using Avalonia.Reactive;

namespace Avalonia.Xaml.Interactions.Core;

/// <summary>
/// A behavior that listens for a specified event on its source and executes its actions when that event is fired.
/// </summary>
public class EventTriggerBehavior : StyledElementTrigger
{
    private const string EventNameDefaultValue = "AttachedToVisualTree";

    /// <summary>
    /// Identifies the <seealso cref="EventName"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<string?> EventNameProperty =
        AvaloniaProperty.Register<EventTriggerBehavior, string?>(nameof(EventName), EventNameDefaultValue);

    /// <summary>
    /// Identifies the <seealso cref="SourceObject"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<object?> SourceObjectProperty =
        AvaloniaProperty.Register<EventTriggerBehavior, object?>(nameof(SourceObject));

    private object? _resolvedSource;
    private Delegate? _eventHandler;
    private bool _isLoadedEventRegistered;

    /// <summary>
    /// Gets or sets the name of the event to listen for. This is a avalonia property.
    /// </summary>
    public string? EventName
    {
        get => GetValue(EventNameProperty);
        set => SetValue(EventNameProperty, value);
    }

    /// <summary>
    /// Gets or sets the source object from which this behavior listens for events.
    /// If <seealso cref="SourceObject"/> is not set, the source will default to <seealso cref="IBehavior.AssociatedObject"/>. This is a avalonia property.
    /// </summary>
    [ResolveByName]
    public object? SourceObject
    {
        get => GetValue(SourceObjectProperty);
        set => SetValue(SourceObjectProperty, value);
    }

    static EventTriggerBehavior()
    {
        EventNameProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<string>>(EventNameChanged));

        SourceObjectProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<object?>>(SourceObjectChanged));
    }

    [RequiresUnreferencedCode("This functionality is not compatible with trimming.")]
    private static void EventNameChanged(AvaloniaPropertyChangedEventArgs<string> e)
    {
        if (e.Sender is not EventTriggerBehavior behavior)
        {
            return;
        }

        if (behavior.AssociatedObject is null || behavior._resolvedSource is null)
        {
            return;
        }

        var oldEventName = e.OldValue.GetValueOrDefault();
        var newEventName = e.NewValue.GetValueOrDefault();

        if (oldEventName is not null)
        {
            behavior.UnregisterEvent(oldEventName);
        }

        if (newEventName is not null)
        {
            behavior.RegisterEvent(newEventName);
        }
    }

    private static void SourceObjectChanged(AvaloniaPropertyChangedEventArgs<object?> e)
    {
        if (e.Sender is EventTriggerBehavior behavior)
        {
            behavior.SetResolvedSource(behavior.ComputeResolvedSource());
        }
    }

    /// <summary>
    /// Called after the behavior is attached to the <see cref="IBehavior.AssociatedObject"/>.
    /// </summary>
    protected override void OnAttached()
    {
        base.OnAttached();
        SetResolvedSource(ComputeResolvedSource());
    }

    /// <summary>
    /// Called when the behavior is being detached from its <see cref="IBehavior.AssociatedObject"/>.
    /// </summary>
    protected override void OnDetaching()
    {
        base.OnDetaching();
        SetResolvedSource(null);
    }

    private void SetResolvedSource(object? newSource)
    {
        if (AssociatedObject is null || _resolvedSource == newSource)
        {
            return;
        }

        if (_resolvedSource is not null)
        {
            UnregisterEvent(EventName);
        }

        _resolvedSource = newSource;

        if (_resolvedSource is not null)
        {
            RegisterEvent(EventName);
        }
    }

    private object? ComputeResolvedSource()
    {
        // If the SourceObject property is set at all, we want to use it. It is possible that it is data
        // bound and bindings haven't been evaluated yet. Plus, this makes the API more predictable.
        if (GetValue(SourceObjectProperty) is not null)
        {
            return SourceObject;
        }

        return AssociatedObject;
    }

    [RequiresUnreferencedCode("This functionality is not compatible with trimming.")]
    private void RegisterEvent(string eventName)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            return;
        }

        if (eventName != EventNameDefaultValue)
        {
            if (_resolvedSource is null)
            {
                return;
            }

            if (EventName is null)
            {
                return;
            }
            
            var sourceObjectType = _resolvedSource.GetType();
            var eventInfo = sourceObjectType.GetRuntimeEvent(EventName);
            if (eventInfo is null)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot find an event named {0} on type {1}.",
                    EventName,
                    sourceObjectType.Name));
            }

            var methodInfo = typeof(EventTriggerBehavior).GetTypeInfo().GetDeclaredMethod("AttachedToVisualTree");
            if (methodInfo is not null)
            {
                var eventHandlerType = eventInfo.EventHandlerType;
                if (eventHandlerType is not null)
                {
                    _eventHandler = methodInfo.CreateDelegate(eventHandlerType, this);
                    if (_eventHandler is not null)
                    {
                        eventInfo.AddEventHandler(_resolvedSource, _eventHandler);
                    }
                }
            }
        }
        else if (!_isLoadedEventRegistered)
        {
            if (_resolvedSource is Control element && !IsElementLoaded(element))
            {
                _isLoadedEventRegistered = true;
                element.AttachedToVisualTree += AttachedToVisualTree;
            }
        }
    }

    [RequiresUnreferencedCode("This functionality is not compatible with trimming.")]
    private void UnregisterEvent(string eventName)
    {
        if (string.IsNullOrEmpty(eventName))
        {
            return;
        }

        if (eventName != EventNameDefaultValue)
        {
            if (_eventHandler is null)
            {
                return;
            }

            if (_resolvedSource is not null)
            {
                var eventInfo = _resolvedSource.GetType().GetRuntimeEvent(eventName);
                eventInfo?.RemoveEventHandler(_resolvedSource, _eventHandler); 
            }
            _eventHandler = null;
        }
        else if (_isLoadedEventRegistered)
        {
            _isLoadedEventRegistered = false;
            if (_resolvedSource is Control element)
            {
                element.AttachedToVisualTree -= AttachedToVisualTree; 
            }
        }
    }

    /// <summary>
    /// Raised when the control is attached to a rooted visual tree.
    /// </summary>
    /// <param name="sender">The sender object.</param>
    /// <param name="eventArgs">The event args.</param>
    protected virtual void AttachedToVisualTree(object? sender, object eventArgs)
    {
        if (!IsEnabled)
        {
            return;
        }

        Interaction.ExecuteActions(_resolvedSource, Actions, eventArgs);
    }

    private static bool IsElementLoaded(Control element) => element.Parent is not null;
}
