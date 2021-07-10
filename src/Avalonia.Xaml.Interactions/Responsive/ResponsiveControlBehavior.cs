using System;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Metadata;
using Avalonia.Xaml.Interactions.Core;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Responsive
{
    /// <summary>
    /// Observes attached control Bounds property changes and if triggered sets or removes style classes.
    /// </summary>
    public sealed class ResponsiveControlBehavior : Behavior<Control>
    {
        private IDisposable? _disposable;
        private AvaloniaList<ResponsiveClassSetter>? _setters;

        /// <summary>
        /// Identifies the <seealso cref="Control"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<Control?> ControlProperty =
            AvaloniaProperty.Register<ResponsiveControlBehavior, Control?>(nameof(Control));

        /// <summary>
        /// Identifies the <seealso cref="Setters"/> avalonia property.
        /// </summary>
        public static readonly DirectProperty<ResponsiveControlBehavior, AvaloniaList<ResponsiveClassSetter>?> SettersProperty = 
            AvaloniaProperty.RegisterDirect<ResponsiveControlBehavior, AvaloniaList<ResponsiveClassSetter>?>(nameof(Setters), t => t._setters);

        /// <summary>
        /// Gets or sets the target control that class name that should be added or removed when triggered. This is a avalonia property.
        /// </summary>
        public Control? Control
        {
            get => GetValue(ControlProperty);
            set => SetValue(ControlProperty, value);
        }

        /// <summary>
        /// Gets responsive setters collection. This is a avalonia property.
        /// </summary>
        [Content]
        public AvaloniaList<ResponsiveClassSetter>? Setters => _setters ??= new AvaloniaList<ResponsiveClassSetter>();

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            StopObserving();
            StartObserving();
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            StopObserving();
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged<T>(AvaloniaPropertyChangedEventArgs<T> change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == ControlProperty)
            {
                StopObserving();
                StartObserving();
            }
            else if (change.Property == SettersProperty)
            {
                StopObserving();
                StartObserving();
            }
        }

        private void StartObserving()
        {
            var target = GetValue(ControlProperty) is { } ? Control : AssociatedObject;
            var setters = Setters;

            if (target is not null && setters is not null)
            {
                _disposable = ObserveBounds(target);
            }
        }

        private void StopObserving()
        {
            _disposable?.Dispose();
        }

        private IDisposable? ObserveBounds(Control target)
        {
            if (target is null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            return target.GetObservable(Visual.BoundsProperty)
                         .Subscribe(bounds => ValueChanged(target, Setters, bounds));
        }

        private void ValueChanged(Control? target, AvaloniaList<ResponsiveClassSetter>? setters, Rect bounds)
        {
            if (target is null || setters is null)
            {
                return;
            }

            foreach (var setter in setters)
            {
                var minValue = setter.Minimum;
                var maxValue = setter.Maximum;

                var property = setter.BoundsProperty switch
                {
                    ResponsiveBoundsProperty.Width => bounds.Width,
                    ResponsiveBoundsProperty.Height => bounds.Height,
                    _ => throw new Exception("Invalid Bounds property.")
                };

                var enabled =
                    GetResult(setter.MinimumOperator, property, minValue)
                    && GetResult(setter.MaximumOperator, property, maxValue);

                var className = setter.ClassName;
                var isPseudoClass = setter.IsPseudoClass;

                if (enabled)
                {
                    Add(target, className, isPseudoClass);
                }
                else
                {
                    Remove(target, className, isPseudoClass);
                }
            }
        }

        private bool GetResult(ComparisonConditionType comparisonConditionType, double property, double value)
        {
            return comparisonConditionType switch
            {
                ComparisonConditionType.Equal => property == value,
                ComparisonConditionType.NotEqual => property != value,
                ComparisonConditionType.LessThan => property < value,
                ComparisonConditionType.LessThanOrEqual => property <= value,
                ComparisonConditionType.GreaterThan => property > value,
                ComparisonConditionType.GreaterThanOrEqual => property >= value,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static void Add(Control target, string? className, bool isPseudoClass)
        {
            if (string.IsNullOrEmpty(className) || target.Classes.Contains(className))
            {
                return;
            }

            if (isPseudoClass)
            {
                ((IPseudoClasses) target.Classes).Add(className);
            }
            else
            {
                target.Classes.Add(className);
            }
        }

        private static void Remove(Control target, string? className, bool isPseudoClass)
        {
            if (string.IsNullOrEmpty(className) || !target.Classes.Contains(className))
            {
                return;
            }

            if (isPseudoClass)
            {
                ((IPseudoClasses) target.Classes).Remove(className);
            }
            else
            {
                target.Classes.Remove(className);
            }
        }
    }
}
