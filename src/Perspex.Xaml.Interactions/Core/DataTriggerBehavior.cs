// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Globalization;
using Perspex.Xaml.Interactivity;
using Perspex.Metadata;

namespace Perspex.Xaml.Interactions.Core
{
    /// <summary>
    /// A behavior that performs actions when the bound data meets a specified condition.
    /// </summary>
    public sealed class DataTriggerBehavior : Behavior
    {
        static DataTriggerBehavior()
        {
            BindingProperty.Changed.Subscribe(e => OnValueChanged(e.Sender, e));
            ComparisonConditionProperty.Changed.Subscribe(e => OnValueChanged(e.Sender, e));
            ValueProperty.Changed.Subscribe(e => OnValueChanged(e.Sender, e));
        }

        /// <summary>
        /// Identifies the <seealso cref="Actions"/> perspex property.
        /// </summary>
        public static readonly PerspexProperty<ActionCollection> ActionsProperty =
            PerspexProperty.Register<DataTriggerBehavior, ActionCollection>(nameof(Actions));

        /// <summary>
        /// Identifies the <seealso cref="Binding"/> perspex property.
        /// </summary>
        public static readonly PerspexProperty<object> BindingProperty =
            PerspexProperty.Register<DataTriggerBehavior, object>(nameof(Binding));

        /// <summary>
        /// Identifies the <seealso cref="ComparisonCondition"/> perspex property.
        /// </summary>
        public static readonly PerspexProperty<ComparisonConditionType> ComparisonConditionProperty =
            PerspexProperty.Register<DataTriggerBehavior, ComparisonConditionType>(nameof(ComparisonCondition), ComparisonConditionType.Equal);

        /// <summary>
        /// Identifies the <seealso cref="Value"/> perspex property.
        /// </summary>
        public static readonly PerspexProperty<object> ValueProperty =
            PerspexProperty.Register<DataTriggerBehavior, object>(nameof(Value));

        /// <summary>
        /// Gets the collection of actions associated with the behavior. This is a perspex property.
        /// </summary>
        [Content]
        public ActionCollection Actions
        {
            get
            {
                ActionCollection actionCollection = this.GetValue(ActionsProperty);
                if (actionCollection == null)
                {
                    actionCollection = new ActionCollection();
                    this.SetValue(ActionsProperty, actionCollection);
                }

                return actionCollection;
            }
        }

        /// <summary>
        /// Gets or sets the bound object that the <see cref="DataTriggerBehavior"/> will listen to. This is a perspex property.
        /// </summary>
        public object Binding
        {
            get { return this.GetValue(BindingProperty); }
            set { this.SetValue(BindingProperty, value); }
        }

        /// <summary>
        /// Gets or sets the type of comparison to be performed between <see cref="DataTriggerBehavior.Binding"/> and <see cref="DataTriggerBehavior.Value"/>. This is a perspex property.
        /// </summary>
        public ComparisonConditionType ComparisonCondition
        {
            get { return this.GetValue(ComparisonConditionProperty); }
            set { this.SetValue(ComparisonConditionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the value to be compared with the value of <see cref="DataTriggerBehavior.Binding"/>. This is a perspex property.
        /// </summary>
        public object Value
        {
            get { return this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        private static bool Compare(object leftOperand, ComparisonConditionType operatorType, object rightOperand)
        {
            if (leftOperand != null && rightOperand != null)
            {
                rightOperand = TypeConverterHelper.Convert(rightOperand.ToString(), leftOperand.GetType().FullName);
            }

            IComparable leftComparableOperand = leftOperand as IComparable;
            IComparable rightComparableOperand = rightOperand as IComparable;
            if ((leftComparableOperand != null) && (rightComparableOperand != null))
            {
                return EvaluateComparable(leftComparableOperand, operatorType, rightComparableOperand);
            }

            switch (operatorType)
            {
                case ComparisonConditionType.Equal:
                    return object.Equals(leftOperand, rightOperand);

                case ComparisonConditionType.NotEqual:
                    return !object.Equals(leftOperand, rightOperand);

                case ComparisonConditionType.LessThan:
                case ComparisonConditionType.LessThanOrEqual:
                case ComparisonConditionType.GreaterThan:
                case ComparisonConditionType.GreaterThanOrEqual:
                    {
                        if (leftComparableOperand == null && rightComparableOperand == null)
                        {
                            throw new ArgumentException(string.Format(
                                CultureInfo.CurrentCulture,
                                "Binding property of type {0} and Value property of type {1} cannot be used with operator {2}.",
                                leftOperand != null ? leftOperand.GetType().Name : "null",
                                rightOperand != null ? rightOperand.GetType().Name : "null",
                                operatorType.ToString()));
                        }
                        else if (leftComparableOperand == null)
                        {
                            throw new ArgumentException(string.Format(
                                CultureInfo.CurrentCulture,
                                "Binding property of type {0} cannot be used with operator {1}.",
                                leftOperand != null ? leftOperand.GetType().Name : "null",
                                operatorType.ToString()));
                        }
                        else
                        {
                            throw new ArgumentException(string.Format(
                                CultureInfo.CurrentCulture,
                                "Value property of type {0} cannot be used with operator {1}.",
                                rightOperand != null ? rightOperand.GetType().Name : "null",
                                operatorType.ToString()));
                        }
                    }
            }

            return false;
        }

        /// <summary>
        /// Evaluates both operands that implement the IComparable interface.
        /// </summary>
        private static bool EvaluateComparable(IComparable leftOperand, ComparisonConditionType operatorType, IComparable rightOperand)
        {
            object convertedOperand = null;
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

            if (convertedOperand == null)
            {
                return operatorType == ComparisonConditionType.NotEqual;
            }

            int comparison = leftOperand.CompareTo((IComparable)convertedOperand);
            switch (operatorType)
            {
                case ComparisonConditionType.Equal:
                    return comparison == 0;

                case ComparisonConditionType.NotEqual:
                    return comparison != 0;

                case ComparisonConditionType.LessThan:
                    return comparison < 0;

                case ComparisonConditionType.LessThanOrEqual:
                    return comparison <= 0;

                case ComparisonConditionType.GreaterThan:
                    return comparison > 0;

                case ComparisonConditionType.GreaterThanOrEqual:
                    return comparison >= 0;
            }

            return false;
        }

        private static void OnValueChanged(PerspexObject perspexObject, PerspexPropertyChangedEventArgs args)
        {
            DataTriggerBehavior dataTriggerBehavior = (DataTriggerBehavior)perspexObject;
            if (dataTriggerBehavior.AssociatedObject == null)
            {
                return;
            }

            DataBindingHelper.RefreshDataBindingsOnActions(dataTriggerBehavior.Actions);

            // NOTE: In UWP version binding and value null checks are not present, Perspex throws exception as Bindings are null when first initialized.
            var binding = dataTriggerBehavior.Binding;
            var value = dataTriggerBehavior.Value;
            if (binding != null && value != null)
            {
                // Some value has changed--either the binding value, reference value, or the comparison condition. Re-evaluate the equation.
                if (Compare(dataTriggerBehavior.Binding, dataTriggerBehavior.ComparisonCondition, dataTriggerBehavior.Value))
                {
                    Interaction.ExecuteActions(dataTriggerBehavior.AssociatedObject, dataTriggerBehavior.Actions, args);
                }
            }
        }
    }
}
