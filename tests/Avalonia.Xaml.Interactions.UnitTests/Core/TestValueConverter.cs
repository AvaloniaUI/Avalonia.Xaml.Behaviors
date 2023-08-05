using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.UnitTests.Core;

internal class TestValueConverter : IValueConverter
{
    public static readonly TestValueConverter Instance = new ();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is RoutedEventArgs args)
        {
            return args.Source?.GetType().Name;
        }

        throw new ArgumentException("Invalid value type");
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
