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
        /// Identifies the <seealso cref="SourceControl"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<Control?> SourceControlProperty =
            AvaloniaProperty.Register<ResponsiveControlBehavior, Control?>(nameof(SourceControl));

        /// <summary>
        /// Identifies the <seealso cref="TargetControl"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<Control?> TargetControlProperty =
            AvaloniaProperty.Register<ResponsiveControlBehavior, Control?>(nameof(TargetControl));

        /// <summary>
        /// Identifies the <seealso cref="Setters"/> avalonia property.
        /// </summary>
        public static readonly DirectProperty<ResponsiveControlBehavior, AvaloniaList<ResponsiveClassSetter>> SettersProperty = 
            AvaloniaProperty.RegisterDirect<ResponsiveControlBehavior, AvaloniaList<ResponsiveClassSetter>>(nameof(Setters), t => t.Setters);

        /// <summary>
        /// Gets or sets the the source control that <see cref="Visual.BoundsProperty"/> property are observed from, if not set <see cref="Behavior{T}.AssociatedObject"/> is used. This is a avalonia property.
        /// </summary>
        public Control? SourceControl
        {
            get => GetValue(SourceControlProperty);
            set => SetValue(SourceControlProperty, value);
        }

        /// <summary>
        /// Gets or sets the target control that class name that should be added or removed when triggered, if not set <see cref="Behavior{T}.AssociatedObject"/> is used or <see cref="ResponsiveClassSetter.TargetControl"/> from <see cref="ResponsiveClassSetter"/>. This is a avalonia property.
        /// </summary>
        public Control? TargetControl
        {
            get => GetValue(TargetControlProperty);
            set => SetValue(TargetControlProperty, value);
        }

        /// <summary>
        /// Gets responsive setters collection. This is a avalonia property.
        /// </summary>
        [Content]
        public AvaloniaList<ResponsiveClassSetter> Setters => _setters ??= new AvaloniaList<ResponsiveClassSetter>();

        /// <summary>
        /// Called after the behavior is attached to the <see cref="Behavior{T}.AssociatedObject"/>.
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

        private void StartObserving()
        {
            var sourceControl = GetValue(SourceControlProperty) is { } 
                ? SourceControl 
                : AssociatedObject;

            if (sourceControl is not null)
            {
                _disposable = ObserveBounds(sourceControl);
            }
        }

        private void StopObserving()
        {
            _disposable?.Dispose();
        }

        private IDisposable ObserveBounds(Control sourceControl)
        {
            if (sourceControl is null)
            {
                throw new ArgumentNullException(nameof(sourceControl));
            }

            return sourceControl.GetObservable(Visual.BoundsProperty)
                                .Subscribe(bounds => ValueChanged(sourceControl, Setters, bounds));
        }

        private void ValueChanged(Control? sourceControl, AvaloniaList<ResponsiveClassSetter>? setters, Rect bounds)
        {
            if (sourceControl is null || setters is null)
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
                var targetControl = setter.GetValue(ResponsiveClassSetter.TargetControlProperty) is { } 
                    ? setter.TargetControl 
                    : GetValue(TargetControlProperty) is { } 
                        ? TargetControl 
                        : AssociatedObject;

                if (targetControl is { })
                {
                    if (enabled)
                    {
                        Add(targetControl, className, isPseudoClass);
                    }
                    else
                    {
                        Remove(targetControl, className, isPseudoClass);
                    }
                }
                else
                {
                    throw new ArgumentNullException(nameof(targetControl));
                }
            }
        }

        private bool GetResult(ComparisonConditionType comparisonConditionType, double property, double value)
        {
            return comparisonConditionType switch
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                ComparisonConditionType.Equal => property == value,
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                ComparisonConditionType.NotEqual => property != value,
                ComparisonConditionType.LessThan => property < value,
                ComparisonConditionType.LessThanOrEqual => property <= value,
                ComparisonConditionType.GreaterThan => property > value,
                ComparisonConditionType.GreaterThanOrEqual => property >= value,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private static void Add(Control targetControl, string? className, bool isPseudoClass)
        {
            if (string.IsNullOrEmpty(className) || targetControl.Classes.Contains(className!))
            {
                return;
            }

            if (isPseudoClass)
            {
                ((IPseudoClasses) targetControl.Classes).Add(className);
            }
            else
            {
                targetControl.Classes.Add(className!);
            }
        }

        private static void Remove(Control targetControl, string? className, bool isPseudoClass)
        {
            if (string.IsNullOrEmpty(className) || !targetControl.Classes.Contains(className!))
            {
                return;
            }

            if (isPseudoClass)
            {
                ((IPseudoClasses) targetControl.Classes).Remove(className);
            }
            else
            {
                targetControl.Classes.Remove(className!);
            }
        }
    }
}
