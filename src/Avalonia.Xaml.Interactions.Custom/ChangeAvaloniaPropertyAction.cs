using System;
using System.Globalization;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// An action that will change a specified Avalonia property to a specified value when invoked.
/// </summary>
public class ChangeAvaloniaPropertyAction : AvaloniaObject, IAction
{
    /// <summary>
    /// Identifies the <seealso cref="TargetProperty"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<AvaloniaProperty?> TargetPropertyProperty =
        AvaloniaProperty.Register<ChangeAvaloniaPropertyAction, AvaloniaProperty?>(nameof(TargetProperty));

    /// <summary>
    /// Identifies the <seealso cref="TargetObject"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<AvaloniaObject?> TargetObjectProperty =
        AvaloniaProperty.Register<ChangeAvaloniaPropertyAction, AvaloniaObject?>(nameof(TargetObject));

    /// <summary>
    /// Identifies the <seealso cref="Value"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<object?> ValueProperty =
        AvaloniaProperty.Register<ChangeAvaloniaPropertyAction, object?>(nameof(Value));

    /// <summary>
    /// Gets or sets the name of the Avalonia property to change. This is a avalonia property.
    /// </summary>
    public AvaloniaProperty? TargetProperty
    {
        get => GetValue(TargetPropertyProperty);
        set => SetValue(TargetPropertyProperty, value);
    }

    /// <summary>
    /// Gets or sets the value to set. This is a avalonia property.
    /// </summary>
    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// Gets or sets the Avalonia object whose property will be changed.
    /// If <seealso cref="TargetObject"/> is not set or cannot be resolved, the sender of <seealso cref="Execute"/> will be used. This is a avalonia property.
    /// </summary>
    [ResolveByName]
    public AvaloniaObject? TargetObject
    {
        get => GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    /// <summary>
    /// Executes the action.
    /// </summary>
    /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
    /// <param name="parameter">The value of this parameter is determined by the caller.</param>
    /// <returns>True if updating the property value succeeds; else false.</returns>
    public virtual object Execute(object? sender, object? parameter)
    {
        var targetObject = GetValue(TargetObjectProperty) is { } ? TargetObject : sender;
        if (targetObject is AvaloniaObject avaloniaObject && TargetProperty is { })
        {
            UpdateAvaloniaPropertyValue(avaloniaObject, TargetProperty);
            return true;
        }

        return false;
    }

    private void UpdateAvaloniaPropertyValue(AvaloniaObject targetObject, AvaloniaProperty targetProperty)
    {
        ValidateTargetProperty(targetProperty);

        Exception? innerException = null;
        try
        {
            object? result = null;
            var propertyType = targetProperty.PropertyType;
            var propertyTypeInfo = propertyType.GetTypeInfo();
            if (Value is null)
            {
                // The result can be null if the type is generic (nullable), or the default value of the type in question
                result = propertyTypeInfo.IsValueType ? Activator.CreateInstance(propertyType) : null;
            }
            else if (propertyTypeInfo.IsAssignableFrom(Value.GetType().GetTypeInfo()))
            {
                result = Value;
            }
            else
            {
                var valueAsString = Value.ToString();
                if (valueAsString is { })
                {
                    result = propertyTypeInfo.IsEnum 
                        ? Enum.Parse(propertyType, valueAsString, false) 
                        : TypeConverterHelper.Convert(valueAsString, propertyType);
                }
            }

            targetObject.SetValue(targetProperty, result);
        }
        catch (FormatException e)
        {
            innerException = e;
        }
        catch (ArgumentException e)
        {
            innerException = e;
        }

        if (innerException is { })
        {
            throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot assign value of type {0} to property {1} of type {2}. The {1} property can be assigned only values of type {2}.",
                    Value?.GetType().Name ?? "null",
                    targetProperty.Name,
                    targetObject.GetType().Name),
                innerException);
        }
    }

    /// <summary>
    /// Ensures the property is not null and can be written to.
    /// </summary>
    private void ValidateTargetProperty(AvaloniaProperty? targetProperty)
    {
        if (targetProperty is null)
        {
            throw new ArgumentException(nameof(TargetProperty));
        }
        else if (targetProperty.IsReadOnly)
        {
            throw new ArgumentException(string.Format(
                CultureInfo.CurrentCulture,
                "Property {0} is read-only.",
                targetProperty.Name));
        }
    }
}
