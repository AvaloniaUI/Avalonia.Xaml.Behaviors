using Avalonia.Controls;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Responsive;

/// <summary>
/// Conditional class setter used in <see cref="AdaptiveBehavior"/> behavior.
/// </summary>
public class AdaptiveClassSetter : AvaloniaObject
{
    /// <summary>
    /// Identifies the <seealso cref="MinWidth"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<double> MinWidthProperty =
        AvaloniaProperty.Register<AdaptiveClassSetter, double>(nameof(MinWidth), 0.0);

    /// <summary>
    /// Identifies the <seealso cref="MinWidthOperator"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<ComparisonConditionType> MinWidthOperatorProperty =
        AvaloniaProperty.Register<AdaptiveClassSetter, ComparisonConditionType>(nameof(MinWidthOperator), ComparisonConditionType.GreaterThanOrEqual);

    /// <summary>
    /// Identifies the <seealso cref="MaxWidth"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<double> MaxWidthProperty =
        AvaloniaProperty.Register<AdaptiveClassSetter, double>(nameof(MaxWidth), double.PositiveInfinity);

    /// <summary>
    /// Identifies the <seealso cref="MaxWidthOperator"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<ComparisonConditionType> MaxWidthOperatorProperty =
        AvaloniaProperty.Register<AdaptiveClassSetter, ComparisonConditionType>(nameof(MaxWidthOperator), ComparisonConditionType.LessThan);

    /// <summary>
    /// Identifies the <seealso cref="MinHeight"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<double> MinHeightProperty =
        AvaloniaProperty.Register<AdaptiveClassSetter, double>(nameof(MinHeight), 0.0);

    /// <summary>
    /// Identifies the <seealso cref="MinHeightOperator"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<ComparisonConditionType> MinHeightOperatorProperty =
        AvaloniaProperty.Register<AdaptiveClassSetter, ComparisonConditionType>(nameof(MinHeightOperator), ComparisonConditionType.GreaterThanOrEqual);

    /// <summary>
    /// Identifies the <seealso cref="MaxHeight"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<double> MaxHeightProperty =
        AvaloniaProperty.Register<AdaptiveClassSetter, double>(nameof(MaxHeight), double.PositiveInfinity);

    /// <summary>
    /// Identifies the <seealso cref="MaxHeightOperator"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<ComparisonConditionType> MaxHeightOperatorProperty =
        AvaloniaProperty.Register<AdaptiveClassSetter, ComparisonConditionType>(nameof(MaxHeightOperator), ComparisonConditionType.LessThan);

    /// <summary>
    /// Identifies the <seealso cref="ClassName"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<string?> ClassNameProperty =
        AvaloniaProperty.Register<AdaptiveClassSetter, string?>(nameof(ClassName));

    /// <summary>
    /// Identifies the <seealso cref="IsPseudoClass"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<bool> IsPseudoClassProperty =
        AvaloniaProperty.Register<AdaptiveClassSetter, bool>(nameof(IsPseudoClass));

    /// <summary>
    /// Identifies the <seealso cref="TargetControl"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<Control?> TargetControlProperty =
        AvaloniaProperty.Register<AdaptiveClassSetter, Control?>(nameof(TargetControl));
        
    /// <summary>
    /// Gets or sets minimum bounds width value used for property comparison. This is a avalonia property.
    /// </summary>
    public double MinWidth
    {
        get => GetValue(MinWidthProperty);
        set => SetValue(MinWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets minimum bounds width value comparison operator. This is a avalonia property.
    /// </summary>
    public ComparisonConditionType MinWidthOperator
    {
        get => GetValue(MinWidthOperatorProperty);
        set => SetValue(MinWidthOperatorProperty, value);
    }

    /// <summary>
    /// Gets or sets maximum width value used for property comparison. This is a avalonia property.
    /// </summary>
    public double MaxWidth
    {
        get => GetValue(MaxWidthProperty);
        set => SetValue(MaxWidthProperty, value);
    }

    /// <summary>
    /// Gets or sets maximum bounds width value comparison operator. This is a avalonia property.
    /// </summary>
    public ComparisonConditionType MaxWidthOperator
    {
        get => GetValue(MaxWidthOperatorProperty);
        set => SetValue(MaxWidthOperatorProperty, value);
    }

    /// <summary>
    /// Gets or sets minimum bounds height value used for property comparison. This is a avalonia property.
    /// </summary>
    public double MinHeight
    {
        get => GetValue(MinHeightProperty);
        set => SetValue(MinHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets minimum bounds height value comparison operator. This is a avalonia property.
    /// </summary>
    public ComparisonConditionType MinHeightOperator
    {
        get => GetValue(MinHeightOperatorProperty);
        set => SetValue(MinHeightOperatorProperty, value);
    }

    /// <summary>
    /// Gets or sets maximum height value used for property comparison. This is a avalonia property.
    /// </summary>
    public double MaxHeight
    {
        get => GetValue(MaxHeightProperty);
        set => SetValue(MaxHeightProperty, value);
    }

    /// <summary>
    /// Gets or sets maximum bounds height value comparison operator. This is a avalonia property.
    /// </summary>
    public ComparisonConditionType MaxHeightOperator
    {
        get => GetValue(MaxHeightOperatorProperty);
        set => SetValue(MaxHeightOperatorProperty, value);
    }

    /// <summary>
    /// Gets or sets the class name that should be added or removed. This is a avalonia property.
    /// </summary>
    [Content]
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

    /// <summary>
    /// Gets or sets the target control that class name that should be added or removed when triggered, if not set <see cref="Behavior{T}.AssociatedObject"/> is used or <see cref="AdaptiveBehavior.TargetControl"/> from <see cref="AdaptiveBehavior"/>. This is a avalonia property.
    /// </summary>
    [ResolveByName]
    public Control? TargetControl
    {
        get => GetValue(TargetControlProperty);
        set => SetValue(TargetControlProperty, value);
    }
}
