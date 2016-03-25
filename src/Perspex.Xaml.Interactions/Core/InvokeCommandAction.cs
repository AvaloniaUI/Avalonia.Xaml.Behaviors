// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Windows.Input;
using Perspex.Xaml.Interactivity;
using Perspex.Markup;

namespace Perspex.Xaml.Interactions.Core
{
    /// <summary>
    /// Executes a specified <see cref="System.Windows.Input.ICommand"/> when invoked. 
    /// </summary>
    public sealed class InvokeCommandAction : PerspexObject, IAction
    {
        /// <summary>
        /// Identifies the <seealso cref="Command"/> perspex property.
        /// </summary>
        public static readonly PerspexProperty<ICommand> CommandProperty =
            PerspexProperty.Register<InvokeCommandAction, ICommand>(nameof(Command));

        /// <summary>
        /// Identifies the <seealso cref="CommandParameter"/> perspex property.
        /// </summary>
        public static readonly PerspexProperty<object> CommandParameterProperty =
            PerspexProperty.Register<InvokeCommandAction, object>(nameof(CommandParameter));

        /// <summary>
        /// Identifies the <seealso cref="InputConverter"/> perspex property.
        /// </summary>
        public static readonly PerspexProperty<IValueConverter> InputConverterProperty =
            PerspexProperty.Register<InvokeCommandAction, IValueConverter>(nameof(InputConverter));

        /// <summary>
        /// Identifies the <seealso cref="InputConverterParameter"/> perspex property.
        /// </summary>
        public static readonly PerspexProperty<object> InputConverterParameterProperty =
            PerspexProperty.Register<InvokeCommandAction, object>(nameof(InputConverterParameter));

        /// <summary>
        /// Identifies the <seealso cref="InputConverterLanguage"/> perspex property.
        /// </summary>
        /// <remarks>The string.Empty used for default value string means the invariant culture.</remarks>
        public static readonly PerspexProperty<string> InputConverterLanguageProperty =
            PerspexProperty.Register<InvokeCommandAction, string>(nameof(InputConverterLanguage), string.Empty);

        /// <summary>
        /// Gets or sets the command this action should invoke. This is a perspex property.
        /// </summary>
        public ICommand Command
        {
            get { return this.GetValue(CommandProperty); }
            set { this.SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter that is passed to <see cref="System.Windows.Input.ICommand.Execute(object)"/>.
        /// If this is not set, the parameter from the <seealso cref="Execute(object, object)"/> method will be used.
        /// This is an optional perspex property.
        /// </summary>
        public object CommandParameter
        {
            get { return this.GetValue(CommandParameterProperty); }
            set { this.SetValue(CommandParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the converter that is run on the parameter from the <seealso cref="Execute(object, object)"/> method.
        /// This is an optional perspex property.
        /// </summary>
        public IValueConverter InputConverter
        {
            get { return this.GetValue(InputConverterProperty); }
            set { this.SetValue(InputConverterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the parameter that is passed to the <see cref="IValueConverter.Convert"/>
        /// method of <see cref="InputConverter"/>.
        /// This is an optional perspex property.
        /// </summary>
        public object InputConverterParameter
        {
            get { return this.GetValue(InputConverterParameterProperty); }
            set { this.SetValue(InputConverterParameterProperty, value); }
        }

        /// <summary>
        /// Gets or sets the language that is passed to the <see cref="IValueConverter.Convert"/>
        /// method of <see cref="InputConverter"/>.
        /// This is an optional perspex property.
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
