// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Input;
using Avalonia.Xaml.Interactivity;
using Avalonia.Data.Converters;

namespace Avalonia.Xaml.Interactions.Core
{
    /// <summary>
    /// Executes a specified <see cref="System.Windows.Input.ICommand"/> when invoked. 
    /// </summary>
    public sealed class InvokeCommandAction : AvaloniaObject, IAction
    {
        /// <summary>
        /// Identifies the <seealso cref="Command"/> avalonia property.
        /// </summary>
        public static readonly AvaloniaProperty<ICommand> CommandProperty =
            AvaloniaProperty.Register<InvokeCommandAction, ICommand>(nameof(Command));

        /// <summary>
        /// Identifies the <seealso cref="CommandParameter"/> avalonia property.
        /// </summary>
        public static readonly AvaloniaProperty<object> CommandParameterProperty =
            AvaloniaProperty.Register<InvokeCommandAction, object>(nameof(CommandParameter));

        /// <summary>
        /// Identifies the <seealso cref="InputConverter"/> avalonia property.
        /// </summary>
        public static readonly AvaloniaProperty<IValueConverter> InputConverterProperty =
            AvaloniaProperty.Register<InvokeCommandAction, IValueConverter>(nameof(InputConverter));

        /// <summary>
        /// Identifies the <seealso cref="InputConverterParameter"/> avalonia property.
        /// </summary>
        public static readonly AvaloniaProperty<object> InputConverterParameterProperty =
            AvaloniaProperty.Register<InvokeCommandAction, object>(nameof(InputConverterParameter));

        /// <summary>
        /// Identifies the <seealso cref="InputConverterLanguage"/> avalonia property.
        /// </summary>
        /// <remarks>The string.Empty used for default value string means the invariant culture.</remarks>
        public static readonly AvaloniaProperty<string> InputConverterLanguageProperty =
            AvaloniaProperty.Register<InvokeCommandAction, string>(nameof(InputConverterLanguage), string.Empty);

        /// <summary>
        /// Gets or sets the command this action should invoke. This is a avalonia property.
        /// </summary>
        public ICommand Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the parameter that is passed to <see cref="System.Windows.Input.ICommand.Execute(object)"/>.
        /// If this is not set, the parameter from the <seealso cref="Execute(object, object)"/> method will be used.
        /// This is an optional avalonia property.
        /// </summary>
        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Gets or sets the converter that is run on the parameter from the <seealso cref="Execute(object, object)"/> method.
        /// This is an optional avalonia property.
        /// </summary>
        public IValueConverter InputConverter
        {
            get => GetValue(InputConverterProperty);
            set => SetValue(InputConverterProperty, value);
        }

        /// <summary>
        /// Gets or sets the parameter that is passed to the <see cref="IValueConverter.Convert"/>
        /// method of <see cref="InputConverter"/>.
        /// This is an optional avalonia property.
        /// </summary>
        public object InputConverterParameter
        {
            get => GetValue(InputConverterParameterProperty);
            set => SetValue(InputConverterParameterProperty, value);
        }

        /// <summary>
        /// Gets or sets the language that is passed to the <see cref="IValueConverter.Convert"/>
        /// method of <see cref="InputConverter"/>.
        /// This is an optional avalonia property.
        /// </summary>
        public string InputConverterLanguage
        {
            get => GetValue(InputConverterLanguageProperty);
            set => SetValue(InputConverterLanguageProperty, value);
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>True if the command is successfully executed; else false.</returns>
        public object? Execute(object sender, object parameter)
        {
            if (Command == null)
            {
                return false;
            }

            object resolvedParameter;
            if (CommandParameter != null)
            {
                resolvedParameter = CommandParameter;
            }
            else if (InputConverter != null)
            {
                resolvedParameter = InputConverter.Convert(
                    parameter,
                    typeof(object),
                    InputConverterParameter,
                    new System.Globalization.CultureInfo(InputConverterLanguage));
            }
            else
            {
                resolvedParameter = parameter;
            }

            if (!Command.CanExecute(resolvedParameter))
            {
                return false;
            }

            Command.Execute(resolvedParameter);
            return true;
        }
    }
}
