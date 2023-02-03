using System;
using System.Globalization;
using Avalonia.Reactive;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Core;

/// <summary>
/// A behavior that performs actions when the bound data meets a specified condition.
/// </summary>
public class DataTriggerBehavior : Trigger
{
    /// <summary>
    /// Identifies the <seealso cref="Binding"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<object?> BindingProperty =
        AvaloniaProperty.Register<DataTriggerBehavior, object?>(nameof(Binding));

    /// <summary>
    /// Identifies the <seealso cref="ComparisonCondition"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<ComparisonConditionType> ComparisonConditionProperty =
        AvaloniaProperty.Register<DataTriggerBehavior, ComparisonConditionType>(nameof(ComparisonCondition));

    /// <summary>
    /// Identifies the <seealso cref="Value"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<object?> ValueProperty =
        AvaloniaProperty.Register<DataTriggerBehavior, object?>(nameof(Value));

    /// <summary>
    /// Gets or sets the bound object that the <see cref="DataTriggerBehavior"/> will listen to. This is a avalonia property.
    /// </summary>
    public object? Binding
    {
        get => GetValue(BindingProperty);
        set => SetValue(BindingProperty, value);
    }

    /// <summary>
    /// Gets or sets the type of comparison to be performed between <see cref="DataTriggerBehavior.Binding"/> and <see cref="DataTriggerBehavior.Value"/>. This is a avalonia property.
    /// </summary>
    public ComparisonConditionType ComparisonCondition
    {
        get => GetValue(ComparisonConditionProperty);
        set => SetValue(ComparisonConditionProperty, value);
    }

    /// <summary>
    /// Gets or sets the value to be compared with the value of <see cref="DataTriggerBehavior.Binding"/>. This is a avalonia property.
    /// </summary>
    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    static DataTriggerBehavior()
    {
        BindingProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<object?>>(OnValueChanged));

        ComparisonConditionProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<ComparisonConditionType>>(OnValueChanged));

        ValueProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<object?>>(OnValueChanged));
    }

    private static bool Compare(object? leftOperand, ComparisonConditionType operatorType, object? rightOperand)
    {
        if (leftOperand is { } && rightOperand is { })
        {
            var value = rightOperand.ToString();
            var destinationType = leftOperand.GetType();
            if (value is { })
            {
                rightOperand = TypeConverterHelper.Convert(value, destinationType);
            }
        }

        var leftComparableOperand = leftOperand as IComparable;
        var rightComparableOperand = rightOperand as IComparable;
        if (leftComparableOperand is { } && rightComparableOperand is { })
        {
            return EvaluateComparable(leftComparableOperand, operatorType, rightComparableOperand);
        }

        switch (operatorType)
        {
            case ComparisonConditionType.Equal:
                return Equals(leftOperand, rightOperand);

            case ComparisonConditionType.NotEqual:
                return !Equals(leftOperand, rightOperand);

            case ComparisonConditionType.LessThan:
            case ComparisonConditionType.LessThanOrEqual:
            case ComparisonConditionType.GreaterThan:
            case ComparisonConditionType.GreaterThanOrEqual:
            {
                throw leftComparableOperand switch
                {
                    null when rightComparableOperand is null => new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture,
                        "Binding property of type {0} and Value property of type {1} cannot be used with operator {2}.",
                        leftOperand?.GetType().Name ?? "null", rightOperand?.GetType().Name ?? "null",
                        operatorType.ToString())),
                    null => new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        "Binding property of type {0} cannot be used with operator {1}.",
                        leftOperand?.GetType().Name ?? "null", operatorType.ToString())),
                    _ => new ArgumentException(string.Format(CultureInfo.CurrentCulture,
                        "Value property of type {0} cannot be used with operator {1}.",
                        rightOperand?.GetType().Name ?? "null", operatorType.ToString()))
                };
            }
        }

        return false;
    }

    /// <summary>
    /// Evaluates both operands that implement the IComparable interface.
    /// </summary>
    private static bool EvaluateComparable(IComparable leftOperand, ComparisonConditionType operatorType, IComparable rightOperand)
    {
        object? convertedOperand = null;
        try
        {
            convertedOperand = Convert.ChangeType(rightOperand, leftOperand.GetType(), CultureInfo.CurrentCulture);
        }
        catch (FormatException)
        {
            // FormatException: Convert.ChangeType("hello", typeof(double), ...);
        }
        catch (InvalidCastException)
        {
            // InvalidCastException: Convert.ChangeType(4.0d, typeof(Rectangle), ...);
        }

        if (convertedOperand is null)
        {
            return operatorType == ComparisonConditionType.NotEqual;
        }

        var comparison = leftOperand.CompareTo((IComparable)convertedOperand);
        return operatorType switch
        {
            ComparisonConditionType.Equal => comparison == 0,
            ComparisonConditionType.NotEqual => comparison != 0,
            ComparisonConditionType.LessThan => comparison < 0,
            ComparisonConditionType.LessThanOrEqual => comparison <= 0,
            ComparisonConditionType.GreaterThan => comparison > 0,
            ComparisonConditionType.GreaterThanOrEqual => comparison >= 0,
            _ => false
        };
    }

    private static void OnValueChanged(AvaloniaPropertyChangedEventArgs args)
    {
        if (args.Sender is not DataTriggerBehavior behavior || behavior.AssociatedObject is null)
        {
            return;
        }

        // NOTE: In UWP version binding null check is not present but Avalonia throws exception as Bindings are null when first initialized.
        var binding = behavior.Binding;
        if (binding is { })
        {
            // Some value has changed--either the binding value, reference value, or the comparison condition. Re-evaluate the equation.
            if (Compare(behavior.Binding, behavior.ComparisonCondition, behavior.Value))
            {
                Interaction.ExecuteActions(behavior.AssociatedObject, behavior.Actions, args);
            }
        }
    }
}
