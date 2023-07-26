using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Core;

/// <summary>
/// An action that will change a specified property to a specified value when invoked.
/// </summary>
public class ChangePropertyAction : AvaloniaObject, IAction
{
    private static readonly char[] s_trimChars = { '(', ')' };
    private static readonly char[] s_separator = { '.' };

    private static Type? GetTypeByName(string name)
    {
        return
            AppDomain.CurrentDomain.GetAssemblies()
                .Reverse()
                .Select(assembly => assembly.GetType(name))
                .FirstOrDefault(t => t is not null)
            ??
            AppDomain.CurrentDomain.GetAssemblies()
                .Reverse()
                .SelectMany(assembly => assembly.GetTypes())
                .FirstOrDefault(t => t.Name == name);
    }

    private static AvaloniaProperty? FindAttachedProperty(object? targetObject, string propertyName)
    {
        if (targetObject is null)
        {
            return null;
        }
        
        var propertyNames = propertyName.Trim().Trim(s_trimChars).Split(s_separator);
        if (propertyNames.Length != 2)
        {
            return null;
        }
        var targetPropertyTypeName = propertyNames[0];
        var targetPropertyName = propertyNames[1];
        var targetType = GetTypeByName(targetPropertyTypeName) ?? targetObject.GetType();

        var registeredAttached = AvaloniaPropertyRegistry.Instance.GetRegisteredAttached(targetType);

        foreach (var avaloniaProperty in registeredAttached)
        {
            if (avaloniaProperty.OwnerType.Name == targetPropertyTypeName && avaloniaProperty.Name == targetPropertyName)
            {
                return avaloniaProperty;
            }
        }

        var registeredInherited = AvaloniaPropertyRegistry.Instance.GetRegisteredInherited(targetType);

        foreach (var avaloniaProperty in registeredInherited)
        {
            if (avaloniaProperty.Name == targetPropertyName)
            {
                return avaloniaProperty;
            }
        }

        return null;
    }

    /// <summary>
    /// Identifies the <seealso cref="PropertyName"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<string> PropertyNameProperty =
        AvaloniaProperty.Register<ChangePropertyAction, string>(nameof(PropertyName));

    /// <summary>
    /// Identifies the <seealso cref="TargetObject"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<object?> TargetObjectProperty =
        AvaloniaProperty.Register<ChangePropertyAction, object?>(nameof(TargetObject));

    /// <summary>
    /// Identifies the <seealso cref="Value"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<object?> ValueProperty =
        AvaloniaProperty.Register<ChangePropertyAction, object?>(nameof(Value));

    /// <summary>
    /// Gets or sets the name of the property to change. This is a avalonia property.
    /// </summary>
    public string PropertyName
    {
        get => GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
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
    /// Gets or sets the object whose property will be changed.
    /// If <seealso cref="TargetObject"/> is not set or cannot be resolved, the sender of <seealso cref="Execute"/> will be used. This is a avalonia property.
    /// </summary>
    [ResolveByName]
    public object? TargetObject
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
        object? targetObject;
        if (GetValue(TargetObjectProperty) is { })
        {
            targetObject = TargetObject;
        }
        else
        {
            targetObject = sender;
        }

        if (targetObject is null)
        {
            return false;
        }

        if (targetObject is AvaloniaObject avaloniaObject)
        {
            if (PropertyName.Contains('.'))
            {
                var avaloniaProperty = FindAttachedProperty(targetObject, PropertyName);
                if (avaloniaProperty is { })
                {
                    UpdateAvaloniaPropertyValue(avaloniaObject, avaloniaProperty);
                    return true;
                }

                return false;
            }
            else
            {
                var avaloniaProperty = AvaloniaPropertyRegistry.Instance.FindRegistered(avaloniaObject, PropertyName);
                if (avaloniaProperty is { })
                {
                    UpdateAvaloniaPropertyValue(avaloniaObject, avaloniaProperty);
                    return true;
                }
            }
        }

        UpdatePropertyValue(targetObject);
        return true;
    }

    private void UpdatePropertyValue(object targetObject)
    {
        var targetType = targetObject.GetType();
        var targetTypeName = targetType.Name;
        var propertyInfo = targetType.GetRuntimeProperty(PropertyName);

        if (propertyInfo is null)
        {
            throw new ArgumentException(string.Format(
                CultureInfo.CurrentCulture,
                "Cannot find a property named {0} on type {1}.",
                PropertyName,
                targetTypeName));
        }
        else if (!propertyInfo.CanWrite)
        {
            throw new ArgumentException(string.Format(
                CultureInfo.CurrentCulture,
                "Cannot find a property named {0} on type {1}.",
                PropertyName,
                targetTypeName));
        }

        Exception? innerException = null;
        try
        {
            object? result = null;
            var propertyType = propertyInfo.PropertyType;
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
                    result = propertyTypeInfo.IsEnum ? Enum.Parse(propertyType, valueAsString, false) :
                        TypeConverterHelper.Convert(valueAsString, propertyType);
                }
            }

            propertyInfo.SetValue(targetObject, result, Array.Empty<object>());
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
                    PropertyName,
                    propertyInfo.PropertyType.Name),
                innerException);
        }
    }

    private void UpdateAvaloniaPropertyValue(AvaloniaObject avaloniaObject, AvaloniaProperty property)
    {
        ValidateAvaloniaProperty(property);

        Exception? innerException = null;
        try
        {
            object? result = null;
            var propertyType = property.PropertyType;
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
                    result = propertyTypeInfo.IsEnum ? Enum.Parse(propertyType, valueAsString, false) :
                        TypeConverterHelper.Convert(valueAsString, propertyType);
                }
            }

            avaloniaObject.SetValue(property, result);
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
                    PropertyName,
                    avaloniaObject.GetType().Name),
                innerException);
        }
    }

    /// <summary>
    /// Ensures the property is not null and can be written to.
    /// </summary>
    private void ValidateAvaloniaProperty(AvaloniaProperty? property)
    {
        if (property is null)
        {
            throw new ArgumentException(string.Format(
                CultureInfo.CurrentCulture,
                "Cannot find a property named {0}.",
                PropertyName));
        }
        else if (property.IsReadOnly)
        {
            throw new ArgumentException(string.Format(
                CultureInfo.CurrentCulture,
                "Cannot find a property named {0}.",
                PropertyName));
        }
    }
}
