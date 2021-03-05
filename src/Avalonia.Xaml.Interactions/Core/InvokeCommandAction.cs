﻿using System.Windows.Input;
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
        public static readonly StyledProperty<ICommand?> CommandProperty =
            AvaloniaProperty.Register<InvokeCommandAction, ICommand?>(nameof(Command));

        /// <summary>
        /// Identifies the <seealso cref="CommandParameter"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<object?> CommandParameterProperty =
            AvaloniaProperty.Register<InvokeCommandAction, object?>(nameof(CommandParameter));

        /// <summary>
        /// Identifies the <seealso cref="InputConverter"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<IValueConverter?> InputConverterProperty =
            AvaloniaProperty.Register<InvokeCommandAction, IValueConverter?>(nameof(InputConverter));

        /// <summary>
        /// Identifies the <seealso cref="InputConverterParameter"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<object?> InputConverterParameterProperty =
            AvaloniaProperty.Register<InvokeCommandAction, object?>(nameof(InputConverterParameter));

        /// <summary>
        /// Identifies the <seealso cref="InputConverterLanguage"/> avalonia property.
        /// </summary>
        /// <remarks>The string.Empty used for default value string means the invariant culture.</remarks>
        public static readonly StyledProperty<string?> InputConverterLanguageProperty =
            AvaloniaProperty.Register<InvokeCommandAction, string?>(nameof(InputConverterLanguage), string.Empty);

        /// <summary>
        /// Gets or sets the command this action should invoke. This is a avalonia property.
        /// </summary>
        public ICommand? Command
        {
            get => GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        /// <summary>
        /// Gets or sets the parameter that is passed to <see cref="System.Windows.Input.ICommand.Execute(object)"/>.
        /// If this is not set, the parameter from the <seealso cref="Execute(object, object)"/> method will be used.
        /// This is an optional avalonia property.
        /// </summary>
        public object? CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        /// <summary>
        /// Gets or sets the converter that is run on the parameter from the <seealso cref="Execute(object, object)"/> method.
        /// This is an optional avalonia property.
        /// </summary>
        public IValueConverter? InputConverter
        {
            get => GetValue(InputConverterProperty);
            set => SetValue(InputConverterProperty, value);
        }

        /// <summary>
        /// Gets or sets the parameter that is passed to the <see cref="IValueConverter.Convert"/>
        /// method of <see cref="InputConverter"/>.
        /// This is an optional avalonia property.
        /// </summary>
        public object? InputConverterParameter
        {
            get => GetValue(InputConverterParameterProperty);
            set => SetValue(InputConverterParameterProperty, value);
        }

        /// <summary>
        /// Gets or sets the language that is passed to the <see cref="IValueConverter.Convert"/>
        /// method of <see cref="InputConverter"/>.
        /// This is an optional avalonia property.
        /// </summary>
        public string? InputConverterLanguage
        {
            get => GetValue(InputConverterLanguageProperty);
            set => SetValue(InputConverterLanguageProperty, value);
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="eventArgs">The value of this parameter is determined by the caller.</param>
        /// <returns>True if the command is successfully executed; else false.</returns>
        public object Execute(object? sender, object? eventArgs)
        {
            if (Command is null)
            {
                return false;
            }

            CommandParameters list = new();

            list.Sender = sender;

            if (InputConverter is { })
            {
                list.EventArgs = InputConverter.Convert(
                    eventArgs,
                    typeof(object),
                    InputConverterParameter,
                    InputConverterLanguage is { } ?
                        new System.Globalization.CultureInfo(InputConverterLanguage)
                        : System.Globalization.CultureInfo.CurrentCulture);
            }
            else
            {
                list.EventArgs = eventArgs;

            }

            if (IsSet(CommandParameterProperty))
            {
                list.CommandParameter = CommandParameter;
            }


            if (!Command.CanExecute(list))
            {
                return false;
            }

            Command.Execute(list);
            return true;
        }
    }

    public struct CommandParameters
    {
        public object? Sender;
        public object? EventArgs;
        public object? CommandParameter;
    }
}
