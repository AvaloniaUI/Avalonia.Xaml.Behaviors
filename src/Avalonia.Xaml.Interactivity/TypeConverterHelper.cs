using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// A helper class that enables converting values specified in markup (strings) to their object representation.
/// </summary>
internal static class TypeConverterHelper
{
    /// <summary>
    /// Converts string representation of a value to its object representation.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="destinationType">The destination type.</param>
    /// <returns>Object representation of the string value.</returns>
    /// <exception cref="ArgumentNullException">destinationType cannot be null.</exception>
    public static object? Convert(string value, Type destinationType)
    {
        if (destinationType is null)
        {
            throw new ArgumentNullException(nameof(destinationType));
        }

        var destinationTypeFullName = destinationType.FullName;
        if (destinationTypeFullName is null)
        {
            return null;
        }

        var scope = GetScope(destinationTypeFullName);

        // Value types in the "System" namespace must be special cased due to a bug in the xaml compiler
        if (string.Equals(scope, "System", StringComparison.Ordinal))
        {
            if (string.Equals(destinationTypeFullName, typeof(string).FullName, StringComparison.Ordinal))
            {
                return value;
            }

            if (string.Equals(destinationTypeFullName, typeof(bool).FullName, StringComparison.Ordinal))
            {
                return bool.Parse(value);
            }

            if (string.Equals(destinationTypeFullName, typeof(int).FullName, StringComparison.Ordinal))
            {
                return int.Parse(value, CultureInfo.InvariantCulture);
            }

            if (string.Equals(destinationTypeFullName, typeof(double).FullName, StringComparison.Ordinal))
            {
                return double.Parse(value, CultureInfo.InvariantCulture);
            }
        }

        try
        {
            if (destinationType.BaseType == typeof(Enum))
                return Enum.Parse(destinationType, value);

            if (destinationType.GetInterfaces().Any(t => t == typeof(IConvertible)))
            {
                return (value as IConvertible).ToType(destinationType, CultureInfo.InvariantCulture);
            }

            var converter = TypeDescriptor.GetConverter(destinationType);
            return converter.ConvertFromInvariantString(value);
        }
        catch (ArgumentException)
        {
            // not an enum
        }
        catch (InvalidCastException)
        {
            // not able to convert to anything
        }

        return null;
    }

    private static string GetScope(string name)
    {
        var indexOfLastPeriod = name.LastIndexOf('.');
        if (indexOfLastPeriod != name.Length - 1)
        {
            return name.Substring(0, indexOfLastPeriod);
        }

        return name;
    }
}
