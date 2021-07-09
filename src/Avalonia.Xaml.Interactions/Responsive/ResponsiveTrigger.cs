using Avalonia.Xaml.Interactions.Core;

namespace Avalonia.Xaml.Interactions.Responsive
{
    /// <summary>
    /// Responsive trigger serves as input condition for <see cref="ResponsiveControlBehavior"/> behavior.
    /// </summary>
    public class ResponsiveTrigger : AvaloniaObject
    {
        /// <summary>
        /// Identifies the <seealso cref="MinValue"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<double> MinValueProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, double>(nameof(MinValue));
        
        /// <summary>
        /// Identifies the <seealso cref="MinOperator"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<ComparisonConditionType> MinOperatorProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, ComparisonConditionType>(nameof(MinOperator));

        /// <summary>
        /// Identifies the <seealso cref="MaxValue"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<double> MaxValueProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, double>(nameof(MaxValue));

        /// <summary>
        /// Identifies the <seealso cref="MaxOperator"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<ComparisonConditionType> MaxOperatorProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, ComparisonConditionType>(nameof(MaxOperator));

        /// <summary>
        /// Identifies the <seealso cref="Property"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<ResponsiveBoundsProperty> PropertyProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, ResponsiveBoundsProperty>(nameof(Property));

        /// <summary>
        /// Identifies the <seealso cref="ClassName"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<string> ClassNameProperty =
            AvaloniaProperty.Register<ResponsiveTrigger, string>(nameof(ClassName));

        /// <summary>
        /// Gets or sets minimum value used for property comparison. This is a avalonia property.
        /// </summary>
        public double MinValue
        {
            get => GetValue(MinValueProperty);
            set => SetValue(MinValueProperty, value);
        }

        /// <summary>
        /// Gets or sets minimum value comparison operator. This is a avalonia property.
        /// </summary>
        public ComparisonConditionType MinOperator
        {
            get => GetValue(MinOperatorProperty);
            set => SetValue(MinOperatorProperty, value);
        }

        /// <summary>
        /// Gets or sets maximum value used for property comparison. This is a avalonia property.
        /// </summary>
        public double MaxValue
        {
            get => GetValue(MaxValueProperty);
            set => SetValue(MaxValueProperty, value);
        }

        /// <summary>
        /// Gets or sets maximum value comparison operator. This is a avalonia property.
        /// </summary>
        public ComparisonConditionType MaxOperator
        {
            get => GetValue(MaxOperatorProperty);
            set => SetValue(MaxOperatorProperty, value);
        }

        /// <summary>
        /// Gets or sets Bounds property used for comparison. This is a avalonia property.
        /// </summary>
        public ResponsiveBoundsProperty Property
        {
            get => GetValue(PropertyProperty);
            set => SetValue(PropertyProperty, value);
        }

        /// <summary>
        /// Gets or sets the class name that should be added or removed. This is a avalonia property.
        /// </summary>
        public string ClassName
        {
            get => GetValue(ClassNameProperty);
            set => SetValue(ClassNameProperty, value);
        }
    }
}
