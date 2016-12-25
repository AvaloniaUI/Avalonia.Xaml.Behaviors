// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Input;
using Avalonia.Xaml.Interactivity;
using Avalonia.Markup;

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
            get { return this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter that is passed to <see cref="System.Windows.Input.ICommand.Execute(object)"/>.
        /// If this is not set, the parameter from the <seealso cref="Execute(object, object)"/> method will be used.
        /// This is an optional avalonia property.
        /// </summary>
        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the converter that is run on the parameter from the <seealso cref="Execute(object, object)"/> method.
        /// This is an optional avalonia property.
        /// </summary>
        public IValueConverter InputConverter
        {
            get { return this.GetValue(InputConverterProperty); }
            set { this.SetValue(InputConverterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter that is passed to the <see cref="IValueConverter.Convert"/>
        /// method of <see cref="InputConverter"/>.
        /// This is an optional avalonia property.
        /// </summary>
        public object InputConverterParameter
        {
            get { return this.GetValue(InputConverterParameterProperty); }
            set { this.SetValue(InputConverterParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the language that is passed to the <see cref="IValueConverter.Convert"/>
        /// method of <see cref="InputConverter"/>.
        /// This is an optional avalonia property.
        /// </summary>
        public string InputConverterLanguage
        {
            get { return this.GetValue(InputConverterLanguageProperty); }
            set { this.SetValue(InputConverterLanguageProperty, value); }
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="System.Object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>True if the command is successfully executed; else false.</returns>
        public object Execute(object sender, object parameter)
        {
            if (this.Command == null)
            {
                return false;
            }

            object resolvedParameter;
            if (this.CommandParameter != null)
            {
                resolvedParameter = this.CommandParameter;
            }
            else if (this.InputConverter != null)
            {
                resolvedParameter = this.InputConverter.Convert(
                    parameter,
                    typeof(object),
                    this.InputConverterParameter,
                    new System.Globalization.CultureInfo(this.InputConverterLanguage));
            }
            else
            {
                resolvedParameter = parameter;
            }

            if (!this.Command.CanExecute(resolvedParameter))
            {
                return false;
            }

            this.Command.Execute(resolvedParameter);
            return true;
        }
    }
}
