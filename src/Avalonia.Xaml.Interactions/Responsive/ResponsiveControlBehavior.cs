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
            if (AssociatedObject is { })
            {
                AssociatedObject.AttachedToVisualTree += AttachedToVisualTree;
            }
        }

        /// <summary>
        /// Called when the behavior is being detached from its <see cref="Behavior.AssociatedObject"/>.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject is { })
            {
                AssociatedObject.AttachedToVisualTree -= AttachedToVisualTree;
            }

            _disposable?.Dispose();
        }

        private void AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            var target = GetValue(ControlProperty) is { } ? Control : AssociatedObject;
            var setters = Setters;

            if (target is not null && setters is not null)
            {
                _disposable = ObserveBounds(target, setters);
            }
        }

        private static IDisposable? ObserveBounds(Control target, AvaloniaList<ResponsiveClassSetter> setters)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));

            var data = target.GetObservable(Visual.BoundsProperty);
            return data.Subscribe(bounds =>
            {
                foreach (var setter in setters)
                {
                    var minValue = setter.MinValue;
                    var maxValue = setter.MaxValue;

                    var property = setter.Property switch
                    {
                        ResponsiveBoundsProperty.Width => bounds.Width,
                        ResponsiveBoundsProperty.Height => bounds.Height,
                        _ => throw new Exception("Invalid Bounds property.")
                    };

                    var enabled = 
                        GetResult(setter.MinOperator, property, minValue)
                        && GetResult(setter.MaxOperator, property, maxValue);

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
            });
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

        private static bool GetResult(ComparisonConditionType comparisonConditionType, double property, double value)
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
    }
}
