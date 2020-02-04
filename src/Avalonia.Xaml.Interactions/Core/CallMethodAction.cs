// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Core
{
    /// <summary>
    /// An action that calls a method on a specified object when invoked.
    /// </summary>
    public sealed class CallMethodAction : AvaloniaObject, IAction
    {
        static CallMethodAction()
        {
            MethodNameProperty.Changed.Subscribe(e =>
            {
                CallMethodAction callMethodAction = (CallMethodAction)e.Sender;
                callMethodAction.UpdateMethodDescriptors();
            });

            TargetObjectProperty.Changed.Subscribe(e =>
            {
                CallMethodAction callMethodAction = (CallMethodAction)e.Sender;
                if (e.NewValue != null)
                {
                    Type newType = e.NewValue.GetType();
                    callMethodAction.UpdateTargetType(newType); 
                }
            });
        }

        /// <summary>
        /// Identifies the <seealso cref="MethodName"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<string> MethodNameProperty =
            AvaloniaProperty.Register<CallMethodAction, string>(nameof(MethodName));

        /// <summary>
        /// Identifies the <seealso cref="TargetObject"/> avalonia property.
        /// </summary>
        public static readonly StyledProperty<object> TargetObjectProperty =
            AvaloniaProperty.Register<CallMethodAction, object>(nameof(TargetObject), AvaloniaProperty.UnsetValue);

        private Type? targetObjectType;
        private List<MethodDescriptor> methodDescriptors = new List<MethodDescriptor>();
        private MethodDescriptor? cachedMethodDescriptor;

        /// <summary>
        /// Gets or sets the name of the method to invoke. This is a avalonia property.
        /// </summary>
        public string MethodName
        {
            get => GetValue(MethodNameProperty);
            set => SetValue(MethodNameProperty, value);
        }

        /// <summary>
        /// Gets or sets the object that exposes the method of interest. This is a avalonia property.
        /// </summary>
        public object TargetObject
        {
            get => GetValue(TargetObjectProperty);
            set => SetValue(TargetObjectProperty, value);
        }

        /// <summary>
        /// Executes the action.
        /// </summary>
        /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
        /// <param name="parameter">The value of this parameter is determined by the caller.</param>
        /// <returns>True if the method is called; else false.</returns>
        public object? Execute(object? sender, object? parameter)
        {
            object? target;
            if (GetValue(TargetObjectProperty) != AvaloniaProperty.UnsetValue)
            {
                target = TargetObject;
            }
            else
            {
                target = sender;
            }

            if (target == null || string.IsNullOrEmpty(MethodName))
            {
                return false;
            }

            UpdateTargetType(target.GetType());

            MethodDescriptor? methodDescriptor = FindBestMethod(parameter);
            if (methodDescriptor == null)
            {
                if (TargetObject != null)
                {
                    throw new ArgumentException(string.Format(
                        CultureInfo.CurrentCulture,
                        "Cannot find method named {0} on object of type {1} that matches the expected signature.",
                        MethodName,
                        targetObjectType));
                }

                return false;
            }

            ParameterInfo[] parameters = methodDescriptor.Parameters;
            if (parameters.Length == 0)
            {
                methodDescriptor.MethodInfo.Invoke(target, parameters: null);
                return true;
            }
            else if (parameters.Length == 2)
            {
                methodDescriptor.MethodInfo.Invoke(target, new object[] { target, parameter! });
                return true;
            }

            return false;
        }

        private MethodDescriptor? FindBestMethod(object? parameter)
        {
            if (parameter == null)
            {
                return cachedMethodDescriptor;
            }

            TypeInfo parameterTypeInfo = parameter.GetType().GetTypeInfo();

            if (parameterTypeInfo == null)
            {
                return cachedMethodDescriptor;
            }

            MethodDescriptor? mostDerivedMethod = null;

            // Loop over the methods looking for the one whose type is closest to the type of the given parameter.
            foreach (MethodDescriptor currentMethod in methodDescriptors)
            {
                TypeInfo? currentTypeInfo = currentMethod.SecondParameterTypeInfo;

                if (currentTypeInfo != null && currentTypeInfo.IsAssignableFrom(parameterTypeInfo))
                {
                    if (mostDerivedMethod == null || !currentTypeInfo.IsAssignableFrom(mostDerivedMethod.SecondParameterTypeInfo))
                    {
                        mostDerivedMethod = currentMethod;
                    }
                }
            }

            return mostDerivedMethod ?? cachedMethodDescriptor;
        }

        private void UpdateTargetType(Type newTargetType)
        {
            if (newTargetType == targetObjectType)
            {
                return;
            }

            targetObjectType = newTargetType;

            UpdateMethodDescriptors();
        }

        private void UpdateMethodDescriptors()
        {
            methodDescriptors.Clear();
            cachedMethodDescriptor = null;

            if (string.IsNullOrEmpty(MethodName) || targetObjectType == null)
            {
                return;
            }

            // Find all public methods that match the given name  and have either no parameters,
            // or two parameters where the first is of type Object.
            foreach (MethodInfo method in targetObjectType.GetRuntimeMethods())
            {
                if (string.Equals(method.Name, MethodName, StringComparison.Ordinal)
                    && method.ReturnType == typeof(void)
                    && method.IsPublic)
                {
                    ParameterInfo[] parameters = method.GetParameters();
                    if (parameters.Length == 0)
                    {
                        // There can be only one parameterless method of the given name.
                        cachedMethodDescriptor = new MethodDescriptor(method, parameters);
                    }
                    else if (parameters.Length == 2 && parameters[0].ParameterType == typeof(object))
                    {
                        methodDescriptors.Add(new MethodDescriptor(method, parameters));
                    }
                }
            }

            // We didn't find a parameterless method, so we want to find a method that accepts null
            // as a second parameter, but if we have more than one of these it is ambiguous which
            // we should call, so we do nothing.
            if (cachedMethodDescriptor == null)
            {
                foreach (MethodDescriptor method in methodDescriptors)
                {
                    TypeInfo? typeInfo = method.SecondParameterTypeInfo;
                    if (typeInfo != null && (!typeInfo.IsValueType || (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>))))
                    {
                        if (cachedMethodDescriptor != null)
                        {
                            cachedMethodDescriptor = null;
                            return;
                        }
                        else
                        {
                            cachedMethodDescriptor = method;
                        }
                    }
                }
            }
        }

        [DebuggerDisplay("{MethodInfo}")]
        private class MethodDescriptor
        {
            public MethodDescriptor(MethodInfo methodInfo, ParameterInfo[] methodParameters)
            {
                MethodInfo = methodInfo;
                Parameters = methodParameters;
            }

            public MethodInfo MethodInfo { get; private set; }

            public ParameterInfo[] Parameters { get; private set; }

            public int ParameterCount => Parameters.Length;

            public TypeInfo? SecondParameterTypeInfo
            {
                get
                {
                    if (ParameterCount < 2)
                    {
                        return null;
                    }

                    return Parameters[1].ParameterType.GetTypeInfo();
                }
            }
        }
    }
}
