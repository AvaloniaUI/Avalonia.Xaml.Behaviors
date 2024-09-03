using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Reactive;

namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// Defines a <see cref="BehaviorCollection"/> attached property and provides a method for executing an <seealso cref="ActionCollection"/>.
/// </summary>
public class Interaction
{
    static Interaction()
    {
        BehaviorsProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<BehaviorCollection?>>(BehaviorsChanged));
    }

    /// <summary>
    /// Gets or sets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    public static readonly AttachedProperty<BehaviorCollection?> BehaviorsProperty =
        AvaloniaProperty.RegisterAttached<Interaction, AvaloniaObject, BehaviorCollection?>("Behaviors");

    /// <summary>
    /// Gets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    /// <param name="obj">The <see cref="AvaloniaObject"/> from which to retrieve the <see cref="BehaviorCollection"/>.</param>
    /// <returns>A <see cref="BehaviorCollection"/> containing the behaviors associated with the specified object.</returns>
    public static BehaviorCollection GetBehaviors(AvaloniaObject obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        var behaviorCollection = obj.GetValue(BehaviorsProperty);
        if (behaviorCollection is null)
        {
            behaviorCollection = [];
            obj.SetValue(BehaviorsProperty, behaviorCollection);
            SetVisualTreeEventHandlersInitial(obj);
        }

        return behaviorCollection;
    }

    /// <summary>
    /// Sets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    /// <param name="obj">The <see cref="AvaloniaObject"/> on which to set the <see cref="BehaviorCollection"/>.</param>
    /// <param name="value">The <see cref="BehaviorCollection"/> associated with the object.</param>
    public static void SetBehaviors(AvaloniaObject obj, BehaviorCollection? value)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }
        obj.SetValue(BehaviorsProperty, value);
    }

    /// <summary>
    /// Executes all actions in the <see cref="ActionCollection"/> and returns their results.
    /// </summary>
    /// <param name="sender">The <see cref="object"/> which will be passed on to the action.</param>
    /// <param name="actions">The set of actions to execute.</param>
    /// <param name="parameter">The value of this parameter is determined by the calling behavior.</param>
    /// <returns>Returns the results of the actions.</returns>
    public static IEnumerable<object> ExecuteActions(object? sender, ActionCollection? actions, object? parameter)
    {
        if (actions is null)
        {
            return [];
        }

        var results = new List<object>();

        foreach (var avaloniaObject in actions)
        {
            if (avaloniaObject is not IAction action)
            {
                continue;
            }

            var result = action.Execute(sender, parameter);
            if (result is not null)
            {
                results.Add(result);
            }
        }

        return results;
    }

    private static void BehaviorsChanged(AvaloniaPropertyChangedEventArgs<BehaviorCollection?> e)
    {
        var oldCollection = e.OldValue.GetValueOrDefault();
        var newCollection = e.NewValue.GetValueOrDefault();

        if (oldCollection == newCollection)
        {
            return;
        }

        if (oldCollection is { AssociatedObject: not null})
        {
            oldCollection.Detach();
        }

        if (newCollection is not null)
        {
            newCollection.Attach(e.Sender);
            SetVisualTreeEventHandlersRuntime(e.Sender);
        }
    }

    private static void SetVisualTreeEventHandlersInitial(AvaloniaObject obj)
    {
        if (obj is not Control control)
        {
            return;
        }

        // AttachedToVisualTree / DetachedFromVisualTree

        control.AttachedToVisualTree -= Control_AttachedToVisualTreeRuntime;
        control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeRuntime;
        control.AttachedToVisualTree -= Control_AttachedToVisualTreeInitial;
        control.AttachedToVisualTree += Control_AttachedToVisualTreeInitial;
        control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeInitial;
        control.DetachedFromVisualTree += Control_DetachedFromVisualTreeInitial;

        // AttachedToLogicalTree / DetachedFromLogicalTree

        control.AttachedToLogicalTree -= Control_AttachedToLogicalTreeRuntime;
        control.DetachedFromLogicalTree -= Control_DetachedFromLogicalTreeRuntime;
        control.AttachedToLogicalTree -= Control_AttachedToLogicalTreeInitial;
        control.AttachedToLogicalTree += Control_AttachedToLogicalTreeInitial;
        control.DetachedFromLogicalTree -= Control_DetachedFromLogicalTreeInitial;
        control.DetachedFromLogicalTree += Control_DetachedFromLogicalTreeInitial;

        // Loaded / Unloaded

        control.Loaded -= Control_LoadedRuntime;
        control.Unloaded -= Control_UnloadedRuntime;
        control.Loaded -= Control_LoadedInitial;
        control.Loaded += Control_LoadedInitial;
        control.Unloaded -= Control_UnloadedInitial;
        control.Unloaded += Control_UnloadedInitial;
    }

    private static void SetVisualTreeEventHandlersRuntime(AvaloniaObject obj)
    {
        if (obj is not Control control)
        {
            return;
        }

        // AttachedToVisualTree / DetachedFromVisualTree

        control.AttachedToVisualTree -= Control_AttachedToVisualTreeInitial;
        control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeInitial;
        control.AttachedToVisualTree -= Control_AttachedToVisualTreeRuntime;
        control.AttachedToVisualTree += Control_AttachedToVisualTreeRuntime;
        control.DetachedFromVisualTree -= Control_DetachedFromVisualTreeRuntime;
        control.DetachedFromVisualTree += Control_DetachedFromVisualTreeRuntime;

        // AttachedToLogicalTree / DetachedFromLogicalTree

        control.AttachedToLogicalTree -= Control_AttachedToLogicalTreeInitial;
        control.DetachedFromLogicalTree -= Control_DetachedFromLogicalTreeInitial;
        control.AttachedToLogicalTree -= Control_AttachedToLogicalTreeRuntime;
        control.AttachedToLogicalTree += Control_AttachedToLogicalTreeRuntime;
        control.DetachedFromLogicalTree -= Control_DetachedFromLogicalTreeRuntime;
        control.DetachedFromLogicalTree += Control_DetachedFromLogicalTreeRuntime;

        // Loaded / Unloaded

        control.Loaded -= Control_LoadedInitial;
        control.Unloaded -= Control_UnloadedInitial;
        control.Loaded -= Control_LoadedRuntime;
        control.Loaded += Control_LoadedRuntime;
        control.Unloaded -= Control_UnloadedRuntime;
        control.Unloaded += Control_UnloadedRuntime;
    }

    // AttachedToVisualTree / DetachedFromVisualTree

    private static void Control_AttachedToVisualTreeInitial(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).Attach(d);
        GetBehaviors(d).AttachedToVisualTree();
    }

    private static void Control_DetachedFromVisualTreeInitial(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).DetachedFromVisualTree();
        GetBehaviors(d).Detach();
    }
 
    private static void Control_AttachedToVisualTreeRuntime(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).AttachedToVisualTree();
    }

    private static void Control_DetachedFromVisualTreeRuntime(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).DetachedFromVisualTree();
    }

    // AttachedToLogicalTree / DetachedFromLogicalTree

    private static void Control_AttachedToLogicalTreeInitial(object? sender, LogicalTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).AttachedToLogicalTree();
    }

    private static void Control_DetachedFromLogicalTreeInitial(object? sender, LogicalTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).DetachedFromLogicalTree();
    }
 
    private static void Control_AttachedToLogicalTreeRuntime(object? sender, LogicalTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).AttachedToLogicalTree();
    }

    private static void Control_DetachedFromLogicalTreeRuntime(object? sender, LogicalTreeAttachmentEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).DetachedFromLogicalTree();
    }

    // Loaded / Unloaded

    private static void Control_LoadedInitial(object? sender, RoutedEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).Loaded();
    }

    private static void Control_UnloadedInitial(object? sender, RoutedEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).Unloaded();
    }
 
    private static void Control_LoadedRuntime(object? sender, RoutedEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).Loaded();
    }

    private static void Control_UnloadedRuntime(object? sender, RoutedEventArgs e)
    {
        if (sender is not AvaloniaObject d)
        {
            return;
        }

        GetBehaviors(d).Unloaded();
    }
}
