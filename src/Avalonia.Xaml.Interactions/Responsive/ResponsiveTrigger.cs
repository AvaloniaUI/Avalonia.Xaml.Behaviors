using Avalonia.Xaml.Interactions.Core;

namespace Avalonia.Xaml.Interactions.Responsive
{
    public class ResponsiveTrigger : AvaloniaObject
    {
        public static readonly StyledProperty<double> MinValueProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, double>(nameof(MinValue));
        
        public static readonly StyledProperty<ComparisonConditionType> MinOperatorProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, ComparisonConditionType>(nameof(MinOperator));

        public static readonly StyledProperty<double> MaxValueProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, double>(nameof(MaxValue));

        public static readonly StyledProperty<ComparisonConditionType> MaxOperatorProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, ComparisonConditionType>(nameof(MaxOperator));

        public static readonly StyledProperty<ResponsiveBoundsProperty> PropertyProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, ResponsiveBoundsProperty>(nameof(Property));

        public static readonly StyledProperty<string> ClassNameProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, string>(nameof(ClassName));

        public double MinValue
        {
            get => GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        public ComparisonConditionType MinOperator
        {
            get => GetValue(MinOperatorProperty);
            set => SetValue(MinOperatorProperty, value);
        }

        public double MaxValue
        {
            get => GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        public ComparisonConditionType MaxOperator
        {
            get => GetValue(MaxOperatorProperty);
            set => SetValue(MaxOperatorProperty, value);
        }

        public ResponsiveBoundsProperty Property
        {
            get => GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }

        public string ClassName
        {
            get => GetValue(ClassNameProperty);
            set => SetValue(ClassNameProperty, value);
        }
    }
}
