using Avalonia.Xaml.Interactions.Core;

namespace Avalonia.Xaml.Interactions.Responsive
{
    /// <summary>
    /// Responsive class setter for <see cref="ResponsiveControlBehavior"/> behavior.
    /// </summary>
    public class ResponsiveClassSetter : AvaloniaObject
    {
        /// <summary>
        /// Identifies the <seealso cref="Minimum"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<double> MinimumProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, double>(nameof(Minimum), 0.0);

        /// <summary>
        /// Identifies the <seealso cref="MinimumOperator"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<ComparisonConditionType> MinimumOperatorProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, ComparisonConditionType>(nameof(MinimumOperator), ComparisonConditionType.GreaterThanOrEqual);

        /// <summary>
        /// Identifies the <seealso cref="Maximum"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<double> MaximumProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, double>(nameof(Maximum), double.PositiveInfinity);

        /// <summary>
        /// Identifies the <seealso cref="MaximumOperator"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<ComparisonConditionType> MaximumOperatorProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, ComparisonConditionType>(nameof(MaximumOperator), ComparisonConditionType.LessThan);

        /// <summary>
        /// Identifies the <seealso cref="BoundsProperty"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<ResponsiveBoundsProperty> BoundsPropertyProperty =
            AvaloniaProperty.Register<ResponsiveClassSetter, ResponsiveBoundsProperty>(nameof(BoundsProperty), ResponsiveBoundsProperty.Width);

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
        public double Minimum
        {
            get => GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Gets or sets minimum value comparison operator. This is a avalonia property.
        /// </summary>
        public ComparisonConditionType MinimumOperator
        {
            get => GetValue(MinimumOperatorProperty);
            set => SetValue(MinimumOperatorProperty, value);
        }

        /// <summary>
        /// Gets or sets maximum value used for property comparison. This is a avalonia property.
        /// </summary>
        public double Maximum
        {
            get => GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// Gets or sets maximum value comparison operator. This is a avalonia property.
        /// </summary>
        public ComparisonConditionType MaximumOperator
        {
            get => GetValue(MaximumOperatorProperty);
            set => SetValue(MaximumOperatorProperty, value);
        }

        /// <summary>
        /// Gets or sets Bounds property used for comparison. This is a avalonia property.
        /// </summary>
        public ResponsiveBoundsProperty BoundsProperty
        {
            get => GetValue(BoundsPropertyProperty);
            set => SetValue(BoundsPropertyProperty, value);
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
