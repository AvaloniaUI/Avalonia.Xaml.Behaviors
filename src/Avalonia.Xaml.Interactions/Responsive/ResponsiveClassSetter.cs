using Avalonia.Xaml.Interactions.Core;

namespace Avalonia.Xaml.Interactions.Responsive
{
    /// <summary>
    /// Responsive class setter for <see cref="ResponsiveControlBehavior"/> behavior.
    /// </summary>
    public class ResponsiveClassSetter : AvaloniaObject
    {
        /// <summary>
        /// Identifies the <seealso cref="MinValue"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<double> MinValueProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, double>(nameof(MinValue), 0.0);

        /// <summary>
        /// Identifies the <seealso cref="MinOperator"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<ComparisonConditionType> MinOperatorProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, ComparisonConditionType>(nameof(MinOperator), ComparisonConditionType.GreaterThanOrEqual);

        /// <summary>
        /// Identifies the <seealso cref="MaxValue"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<double> MaxValueProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, double>(nameof(MaxValue), double.PositiveInfinity);

        /// <summary>
        /// Identifies the <seealso cref="MaxOperator"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<ComparisonConditionType> MaxOperatorProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, ComparisonConditionType>(nameof(MaxOperator), ComparisonConditionType.LessThan);

        /// <summary>
        /// Identifies the <seealso cref="Property"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<ResponsiveBoundsProperty> PropertyProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, ResponsiveBoundsProperty>(nameof(Property), ResponsiveBoundsProperty.Width);

        /// <summary>
        /// Identifies the <seealso cref="ClassName"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<string?> ClassNameProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, string?>(nameof(ClassName), default);

        /// <summary>
        /// Identifies the <seealso cref="IsPseudoClass"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<bool> IsPseudoClassProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, bool>(nameof(IsPseudoClass), false);

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
        public string? ClassName
        {
            get => GetValue(ClassNameProperty);
            set => SetValue(ClassNameProperty, value);
        }

        /// <summary>
        /// Gets or sets the flag whether ClassName is a PseudoClass. This is a avalonia property.
        /// </summary>
        public bool IsPseudoClass
        {
            get => GetValue(IsPseudoClassProperty);
            set => SetValue(IsPseudoClassProperty, value);
        }
    }
}
