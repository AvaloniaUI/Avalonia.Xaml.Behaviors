using System;
using System.Collections.Generic;
using Avalonia.Controls;

namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// Defines a <see cref="BehaviorCollection"/> attached property and provides a method for executing an <seealso cref="ActionCollection"/>.
/// </summary>
public class Interaction
{
    static Interaction()
    {
        BehaviorsProperty.Changed.Subscribe(e =>
        {
            var oldCollection = e.OldValue.GetValueOrDefault();
            var newCollection = e.NewValue.GetValueOrDefault();

            if (oldCollection == newCollection)
            {
                return;
            }

            if (oldCollection is { } && oldCollection.AssociatedObject is { })
            {
                oldCollection.Detach();
            }

            if (newCollection is { })
            {
                newCollection.Attach(e.Sender);
                SetVisualTreeEventHandlers2(e.Sender);
            }
        });
    }

    /// <summary>
    /// Gets or sets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    public static readonly AttachedProperty<BehaviorCollection?> BehaviorsProperty =
        AvaloniaProperty.RegisterAttached<Interaction, IAvaloniaObject, BehaviorCollection?>("Behaviors");

    /// <summary>
    /// Gets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    /// <param name="obj">The <see cref="IAvaloniaObject"/> from which to retrieve the <see cref="BehaviorCollection"/>.</param>
    /// <returns>A <see cref="BehaviorCollection"/> containing the behaviors associated with the specified object.</returns>
    public static BehaviorCollection GetBehaviors(IAvaloniaObject obj)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        var behaviorCollection = obj.GetValue(BehaviorsProperty);
        if (behaviorCollection is null)
        {
            behaviorCollection = new BehaviorCollection();
            obj.SetValue(BehaviorsProperty, behaviorCollection);
            SetVisualTreeEventHandlers1(obj);
        }

        return behaviorCollection;
    }

    private static void SetVisualTreeEventHandlers1(IAvaloniaObject obj)
    {
        if (obj is Control control)
        {
            control.AttachedToVisualTree -= Control_AttachedToVisualTree2;
            control.DetachedFromVisualTree -= Control_DetachedFromVisualTree2;

            control.AttachedToVisualTree -= Control_AttachedToVisualTree1;
            control.AttachedToVisualTree += Control_AttachedToVisualTree1;
            control.DetachedFromVisualTree -= Control_DetachedFromVisualTree1;
            control.DetachedFromVisualTree += Control_DetachedFromVisualTree1;
        }
    }

    private static void SetVisualTreeEventHandlers2(IAvaloniaObject obj)
    {
        if (obj is Control control)
        {
            control.AttachedToVisualTree -= Control_AttachedToVisualTree1;
            control.DetachedFromVisualTree -= Control_DetachedFromVisualTree1;

            control.AttachedToVisualTree -= Control_AttachedToVisualTree2;
            control.AttachedToVisualTree += Control_AttachedToVisualTree2;
            control.DetachedFromVisualTree -= Control_DetachedFromVisualTree2;
            control.DetachedFromVisualTree += Control_DetachedFromVisualTree2;
        }
    }

    /// <summary>
    /// Sets the <see cref="BehaviorCollection"/> associated with a specified object.
    /// </summary>
    /// <param name="obj">The <see cref="IAvaloniaObject"/> on which to set the <see cref="BehaviorCollection"/>.</param>
    /// <param name="value">The <see cref="BehaviorCollection"/> associated with the object.</param>
    public static void SetBehaviors(IAvaloniaObject obj, BehaviorCollection? value)
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
        var results = new List<object>();

        if (actions is null)
        {
            return results;
        }

        foreach (var avaloniaObject in actions)
        {
            if (avaloniaObject is IAction action)
            {
                var result = action.Execute(sender, parameter);
                if (result is { })
                {
                    results.Add(result);
                }
            }
        }

        return results;
    }

    private static void Control_AttachedToVisualTree1(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is IAvaloniaObject d)
        {
            GetBehaviors(d).Attach(d);
            GetBehaviors(d).AttachedToVisualTree();
        }
    }

    private static void Control_DetachedFromVisualTree1(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is IAvaloniaObject d)
        {
            GetBehaviors(d).Detach();
            GetBehaviors(d).DetachedFromVisualTree();
        }
    }
 
    private static void Control_AttachedToVisualTree2(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is IAvaloniaObject d)
        {
            GetBehaviors(d).AttachedToVisualTree();
        }
    }

    private static void Control_DetachedFromVisualTree2(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (sender is IAvaloniaObject d)
        {
            GetBehaviors(d).DetachedFromVisualTree();
        }
    }
}
