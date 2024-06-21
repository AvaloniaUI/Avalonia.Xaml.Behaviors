using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom
{
    public enum ConditionType
    {
        If,
        ElseIf,
        Else
    }

    #region If-Else Behavior

    public class IfElseBehavior : Trigger
    {
        static IfElseBehavior()
        {
            BindingProperty.Changed.Subscribe(OnValueChanged);
        }

        /// <summary>
        /// Identifies the <seealso cref="Binding"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<object?> BindingProperty =
            AvaloniaProperty.Register<IfElseBehavior, object?>(nameof(Binding));

        /// <summary>
        /// Gets or sets the bound object that the <see cref="IfElseBehavior"/> will listen to. This is a avalonia property.
        /// </summary>
        public object? Binding
        {
            get => GetValue(BindingProperty);
            set => SetValue(BindingProperty, value);
        }

        public IfElseBehavior()
        {
            Actions.CollectionChanged += Actions_CollectionChanged;
        }

        private void Actions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is not null)
            {
                foreach (var item in e.NewItems.OfType<IfElseActionBase>())
                {
                    item.ParentBehavior = this;
                    item.BindingChanged += Item_BindingChanged;
                }
            }
            if (e.OldItems is not null)
            {
                foreach (var item in e.OldItems.OfType<IfElseActionBase>())
                {
                    item.BindingChanged -= Item_BindingChanged;
                    item.ParentBehavior = null;
                }
            }
        }

        private static void OnValueChanged(AvaloniaPropertyChangedEventArgs args)
        {
            if (args.Sender is not IfElseBehavior behavior || behavior.AssociatedObject is null)
                return;

            behavior.RaiseValueChanged(args);
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            if (AssociatedObject is StyledElement styled)
            {
                if (styled.IsInitialized)
                    Init();
                else
                    styled.Initialized += Styled_Initialized;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (AssociatedObject is StyledElement styled)
                styled.Initialized -= Styled_Initialized;
        }

        private void Styled_Initialized(object? sender, System.EventArgs e)
        {
            Init();
        }

        private void Init() => RaiseValueChanged();

        private void Item_BindingChanged(object? sender, System.EventArgs e)
        {
            RaiseValueChanged();
        }

        protected void RaiseValueChanged() => RaiseValueChanged(null);

        protected void RaiseValueChanged(AvaloniaPropertyChangedEventArgs? args)
        {
            ExecuteIfElseActions(Actions, AssociatedObject, args);
        }

        internal static IEnumerable<object> ExecuteIfElseActions(IEnumerable collection, object? sender, object? args = null)
        {
            List<object> results = new();

            object? ExecuteAction(IAction action, object? sender, object? parameter)
            {
                var result = action.Execute(sender, parameter);
                if (result is not null)
                {
                    results.Add(result);
                }
                return result;
            }

            bool currentState = false;
            foreach (var action in collection.OfType<IAction>())
            {
                // if it is "if-else" action, then we try to compute them
                if (action is IfElseActionBase ifElseAction)
                {
                    switch (ifElseAction.ConditionType)
                    {
                        case ConditionType.If:
                            {
                                // "if" is always executed
                                bool canExecute = ifElseAction.CanExecute();
                                if (canExecute)
                                    ExecuteAction(action, sender, args);

                                currentState = canExecute;
                                break;
                            }

                        case ConditionType.ElseIf:
                            {
                                // "else if" executed if the previous action failed
                                if (currentState)
                                    break;

                                bool canExecute = ifElseAction.CanExecute();
                                if (canExecute)
                                    ExecuteAction(action, sender, args);

                                currentState = canExecute;
                                break;
                            }

                        case ConditionType.Else:
                            {
                                // "else" executed if the previous action failed
                                if (currentState)
                                    break;

                                // at the same time, the conditions are not checked in "else"
                                ExecuteAction(action, sender, args);

                                currentState = true;
                                break;
                            }

                        default:
                            throw new ArgumentOutOfRangeException(nameof(ifElseAction.ConditionType));
                    }
                }
                else // if it is a usual action, then we perform it anyway
                {
                    ExecuteAction(action, sender, args);
                    currentState = false;
                }
            }

            return results;
        }
    }

    #endregion

    #region If-Else Action

    public abstract class IfElseActionBase : AvaloniaObject, IAction
    {
        #region Properties

        /// <summary>
        /// Identifies the <seealso cref="Binding"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<object?> BindingProperty =
            AvaloniaProperty.Register<IfElseActionBase, object?>(nameof(Binding));

        /// <summary>
        /// Identifies the <seealso cref="ComparisonCondition"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<ComparisonConditionType> ComparisonConditionProperty =
            AvaloniaProperty.Register<IfElseActionBase, ComparisonConditionType>(nameof(ComparisonCondition), ComparisonConditionType.Equal);

        /// <summary>
        /// Identifies the <seealso cref="Value"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<object?> ValueProperty =
            AvaloniaProperty.Register<IfElseActionBase, object?>(nameof(Value));

        /// <summary>
        /// Identifies the <seealso cref="Actions"/> avalonia property.
        /// </summary>
        public static readonly DirectProperty<IfElseActionBase, ActionCollection> ActionsProperty =
            AvaloniaProperty.RegisterDirect<IfElseActionBase, ActionCollection>(nameof(Actions), t => t.Actions);

        /// <summary>
        /// Gets or sets the bound object that the <see cref="IfElseActionBase"/> will listen to. This is a avalonia property.
        /// </summary>
        public object? Binding
        {
            get => GetValue(BindingProperty);
            set => SetValue(BindingProperty, value);
        }

        /// <summary>
        /// Gets or sets the type of comparison to be performed between <see cref="IfElseActionBase.Binding"/> and <see cref="IfElseActionBase.Value"/>. This is a avalonia property.
        /// </summary>
        public ComparisonConditionType ComparisonCondition
        {
            get => GetValue(ComparisonConditionProperty);
            set => SetValue(ComparisonConditionProperty, value);
        }

        /// <summary>
        /// Gets or sets the value to be compared with the value of <see cref="IfElseActionBase.Binding"/>. This is a avalonia property.
        /// </summary>
        public object? Value
        {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        private ActionCollection? _actions;

        /// <summary>
        /// Gets the collection of actions associated with the behavior. This is a avalonia property.
        /// </summary>
        [Content]
        public ActionCollection Actions => _actions ??= new ActionCollection();

        internal IfElseBehavior? ParentBehavior { get; set; }
        internal IfElseActionBase? ParentAction { get; set; }

        internal event EventHandler? BindingChanged;

        #endregion

        static IfElseActionBase()
        {
            BindingProperty.Changed.Subscribe(OnBindingChanged);
        }

        public IfElseActionBase()
        {
            Actions.CollectionChanged += Actions_CollectionChanged;
        }

        public ConditionType ConditionType { get; protected set; }

        private static void OnBindingChanged(AvaloniaPropertyChangedEventArgs args)
        {
            if (args.Sender is not IfElseActionBase ifElseAction)
                return;

            ifElseAction.RaiseBindingChanged(args);
        }

        private void RaiseBindingChanged(AvaloniaPropertyChangedEventArgs args)
        {
            BindingChanged?.Invoke(this, args);
            if (ParentBehavior is null)
                ParentAction?.RaiseBindingChanged(args);
        }

        private void Actions_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems is not null)
            {
                foreach (var item in e.NewItems.OfType<IfElseActionBase>())
                    item.ParentAction = this;
            }
            if (e.OldItems is not null)
            {
                foreach (var item in e.OldItems.OfType<IfElseActionBase>())
                    item.ParentAction = null;
            }
        }

        public bool CanExecute() => Compare(Binding ?? GetParentBinding());

        protected virtual bool Compare(object? binding) => Compare(binding, ComparisonCondition, Value);

        protected object? GetParentBinding()
        {
            if (ParentAction is not null)
            {
                return ParentAction.Binding ?? ParentAction.GetParentBinding();
            }

            return ParentBehavior?.Binding;
        }

        public object? Execute(object? sender, object? parameter)
        {
            return IfElseBehavior.ExecuteIfElseActions(Actions, sender, parameter);
        }

        #region Compare

        public static bool Compare(object? leftOperand, ComparisonConditionType operatorType, object? rightOperand)
        {
            if (leftOperand is not null && rightOperand is not null)
            {
                Type leftType = leftOperand.GetType();
                Type rightType = rightOperand.GetType();

                if (leftType != rightType)
                {
                    // cast via converter
                    var value = rightOperand.ToString();
                    if (value is not null)
                    {
                        try
                        {
                            rightOperand = TypeConverterHelper.Convert(value, leftType);
                            if (rightOperand is not null)
                                rightType = rightOperand.GetType();
                        }
                        catch
                        {
                            // nothing
                        }
                    }
                }

                // compare via IComparable
                if (leftOperand is IComparable leftComparableOperand && rightOperand is IComparable rightComparableOperand)
                {
                    if (leftType == rightType)
                        return EvaluateComparable(leftComparableOperand, operatorType, rightComparableOperand);
                    else
                        return ConvertAndEvaluateComparable(leftComparableOperand, operatorType, rightComparableOperand);
                }
            }

            // compare links
            switch (operatorType)
            {
                case ComparisonConditionType.Equal:
                    return Equals(leftOperand, rightOperand);

                case ComparisonConditionType.NotEqual:
                    return !Equals(leftOperand, rightOperand);

                default:
                    return false;
                    //throw new InvalidOperationException();
            }
        }

        private static bool EvaluateComparable(IComparable leftOperand, ComparisonConditionType operatorType, IComparable rightOperand)
        {
            var comparison = leftOperand.CompareTo(rightOperand);
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

        private static bool ConvertAndEvaluateComparable(IComparable leftOperand, ComparisonConditionType operatorType, IComparable rightOperand)
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
            catch
            {
                return false;
            }

            if (convertedOperand is null || convertedOperand is not IComparable comparableOperand)
            {
                return operatorType == ComparisonConditionType.NotEqual;
            }

            return EvaluateComparable(leftOperand, operatorType, comparableOperand);
        }

        #endregion
    }

    public class IfAction : IfElseActionBase
    {
        public IfAction()
        {
            ConditionType = ConditionType.If;
        }
    }

    public class ElseIfAction : IfElseActionBase
    {
        public ElseIfAction()
        {
            ConditionType = ConditionType.ElseIf;
        }
    }

    public class ElseAction : IfElseActionBase
    {
        public ElseAction()
        {
            ConditionType = ConditionType.Else;
        }
    }

    #endregion
}
