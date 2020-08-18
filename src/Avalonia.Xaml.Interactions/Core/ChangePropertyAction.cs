using System;
using System.Globalization;
using System.Reflection;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Core
{
    /// <summary>
    /// An action that will change a specified property to a specified value when invoked.
    /// </summary>
    public sealed class ChangePropertyAction : AvaloniaObject, IAction
    {
        /// <summary>
        /// Identifies the <seealso cref="PropertyName"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<string> PropertyNameProperty =
            AvaloniaProperty.Register<ChangePropertyAction, string>(nameof(PropertyName));

        /// <summary>
        /// Identifies the <seealso cref="TargetObject"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<object?> TargetObjectProperty =
            AvaloniaProperty.Register<ChangePropertyAction, object?>(nameof(TargetObject), null);

        /// <summary>
        /// Identifies the <seealso cref="Value"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<object> ValueProperty =
            AvaloniaProperty.Register<ChangePropertyAction, object>(nameof(Value));

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
        public object Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// Gets or sets the object whose property will be changed.
        /// If <seealso cref="TargetObject"/> is not set or cannot be resolved, the sender of <seealso cref="Execute"/> will be used. This is a avalonia property.
        /// </summary>
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
        public object? Execute(object? sender, object? parameter)
        {
            object? targetObject;
            if (GetValue(TargetObjectProperty) != null)
            {
                targetObject = TargetObject;
            }
            else
            {
                targetObject = sender;
            }

            if (targetObject == null || PropertyName == null)
            {
                return false;
            }

            if (targetObject is IAvaloniaObject avaloniaObject)
            {
                AvaloniaProperty avaloniaProperty = AvaloniaPropertyRegistry.Instance.FindRegistered(avaloniaObject, PropertyName);
                if (avaloniaProperty != null)
                {
                    UpdateAvaloniaPropertyValue(avaloniaObject, avaloniaProperty);
                    return true;
                }
            }

            UpdatePropertyValue(targetObject);
            return true;
        }

        private void UpdatePropertyValue(object targetObject)
        {
            Type targetType = targetObject.GetType();
            PropertyInfo propertyInfo = targetType.GetRuntimeProperty(PropertyName);
            ValidateProperty(targetType.Name, propertyInfo);

            Exception? innerException = null;
            try
            {
                object? result = null;
                string? valueAsString = null;
                Type propertyType = propertyInfo.PropertyType;
                TypeInfo propertyTypeInfo = propertyType.GetTypeInfo();
                if (Value == null)
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
                    valueAsString = Value.ToString();
                    result = propertyTypeInfo.IsEnum ? Enum.Parse(propertyType, valueAsString, false) :
                        TypeConverterHelper.Convert(valueAsString, propertyType);
                }

                propertyInfo.SetValue(targetObject, result, new object[0]);
            }
            catch (FormatException e)
            {
                innerException = e;
            }
            catch (ArgumentException e)
            {
                innerException = e;
            }

            if (innerException != null)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot assign value of type {0} to property {1} of type {2}. The {1} property can be assigned only values of type {2}.",
                    Value != null ? Value.GetType().Name : "null",
                    PropertyName,
                    propertyInfo.PropertyType.Name),
                    innerException);
            }
        }

        /// <summary>
        /// Ensures the property is not null and can be written to.
        /// </summary>
        private void ValidateProperty(string targetTypeName, PropertyInfo propertyInfo)
        {
            if (propertyInfo == null)
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
        }

        private void UpdateAvaloniaPropertyValue(IAvaloniaObject avaloniaObject, AvaloniaProperty property)
        {
            ValidateAvaloniaProperty(property);

            Exception? innerException = null;
            try
            {
                object? result = null;
                string? valueAsString = null;
                Type propertyType = property.PropertyType;
                TypeInfo propertyTypeInfo = propertyType.GetTypeInfo();
                if (Value == null)
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
                    valueAsString = Value.ToString();
                    result = propertyTypeInfo.IsEnum ? Enum.Parse(propertyType, valueAsString, false) :
                        TypeConverterHelper.Convert(valueAsString, propertyType);
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

            if (innerException != null)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot assign value of type {0} to property {1} of type {2}. The {1} property can be assigned only values of type {2}.",
                    Value?.GetType().Name ?? "null",
                    PropertyName,
                    avaloniaObject?.GetType().Name ?? "null"),
                    innerException);
            }
        }

        /// <summary>
        /// Ensures the property is not null and can be written to.
        /// </summary>
        private void ValidateAvaloniaProperty(AvaloniaProperty property)
        {
            if (property == null)
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
}
