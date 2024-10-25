using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Reactive;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Core;

/// <summary>
/// An action that calls a method on a specified object when invoked.
/// </summary>
[RequiresUnreferencedCode("This functionality is not compatible with trimming.")]
public class CallMethodAction : Avalonia.Xaml.Interactivity.Action
{
    private Type? _targetObjectType;
    private readonly List<MethodDescriptor> _methodDescriptors = [];
    private MethodDescriptor? _cachedMethodDescriptor;

    /// <summary>
    /// Identifies the <seealso cref="MethodName"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<string?> MethodNameProperty =
        AvaloniaProperty.Register<CallMethodAction, string?>(nameof(MethodName));

    /// <summary>
    /// Identifies the <seealso cref="TargetObject"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<object?> TargetObjectProperty =
        AvaloniaProperty.Register<CallMethodAction, object?>(nameof(TargetObject));

    /// <summary>
    /// Gets or sets the name of the method to invoke. This is a avalonia property.
    /// </summary>
    public string? MethodName
    {
        get => GetValue(MethodNameProperty);
        set => SetValue(MethodNameProperty, value);
    }

    /// <summary>
    /// Gets or sets the object that exposes the method of interest. This is a avalonia property.
    /// </summary>
    [ResolveByName]
    public object? TargetObject
    {
        get => GetValue(TargetObjectProperty);
        set => SetValue(TargetObjectProperty, value);
    }

    static CallMethodAction()
    {
        MethodNameProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<string>>(MethodNameChanged));

        TargetObjectProperty.Changed.Subscribe(
            new AnonymousObserver<AvaloniaPropertyChangedEventArgs<object?>>(TargetObjectChanged));
    }

    private static void MethodNameChanged(AvaloniaPropertyChangedEventArgs<string> e)
    {
        if (e.Sender is not CallMethodAction callMethodAction)
        {
            return;
        }
        
        callMethodAction.UpdateMethodDescriptors();
    }

    [RequiresUnreferencedCode("This functionality is not compatible with trimming.")]
    private static void TargetObjectChanged(AvaloniaPropertyChangedEventArgs<object?> e)
    {
        if (e.Sender is not CallMethodAction callMethodAction)
        {
            return;
        }

        var newValue = e.NewValue.GetValueOrDefault();
        if (newValue is not null)
        {
            var newType = newValue.GetType();
            callMethodAction.UpdateTargetType(newType);
        }
    }

    /// <summary>
    /// Executes the action.
    /// </summary>
    /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
    /// <param name="parameter">The value of this parameter is determined by the caller.</param>
    /// <returns>True if the method is called; else false.</returns>
    public override object Execute(object? sender, object? parameter)
    {
        if (!IsEnabled)
        {
            return false;
        }

        var target = GetValue(TargetObjectProperty) is not null ? TargetObject : sender;
        if (target is null || string.IsNullOrEmpty(MethodName))
        {
            return false;
        }

        UpdateTargetType(target.GetType());

        var methodDescriptor = FindBestMethod(parameter);
        if (methodDescriptor is null)
        {
            if (TargetObject is not null)
            {
                throw new ArgumentException(string.Format(
                    CultureInfo.CurrentCulture,
                    "Cannot find method named {0} on object of type {1} that matches the expected signature.",
                    MethodName,
                    _targetObjectType));
            }

            return false;
        }

        var parameters = methodDescriptor.Parameters;
        switch (parameters.Length)
        {
            case 0:
                methodDescriptor.MethodInfo.Invoke(target, null);
                return true;
            case 2:
                methodDescriptor.MethodInfo.Invoke(target, [target, parameter!]);
                return true;
            default:
                return false;
        }
    }

    [RequiresUnreferencedCode("This functionality is not compatible with trimming.")]
    private MethodDescriptor? FindBestMethod(object? parameter)
    {
        if (parameter is null)
        {
            return _cachedMethodDescriptor;
        }

        var parameterTypeInfo = parameter.GetType().GetTypeInfo();

        MethodDescriptor? mostDerivedMethod = null;

        // Loop over the methods looking for the one whose type is closest to the type of the given parameter.
        foreach (var currentMethod in _methodDescriptors)
        {
            var currentTypeInfo = currentMethod.SecondParameterTypeInfo;

            if (currentTypeInfo is not null && currentTypeInfo.IsAssignableFrom(parameterTypeInfo))
            {
                if (mostDerivedMethod is null || !currentTypeInfo.IsAssignableFrom(mostDerivedMethod.SecondParameterTypeInfo))
                {
                    mostDerivedMethod = currentMethod;
                }
            }
        }

        return mostDerivedMethod ?? _cachedMethodDescriptor;
    }

    private void UpdateTargetType(Type newTargetType)
    {
        if (newTargetType == _targetObjectType)
        {
            return;
        }

        _targetObjectType = newTargetType;

        UpdateMethodDescriptors();
    }

    [RequiresUnreferencedCode("This functionality is not compatible with trimming.")]
    private void UpdateMethodDescriptors()
    {
        _methodDescriptors.Clear();
        _cachedMethodDescriptor = null;

        if (string.IsNullOrEmpty(MethodName) || _targetObjectType is null)
        {
            return;
        }

        // Find all public methods that match the given name  and have either no parameters,
        // or two parameters where the first is of type Object.
        foreach (var method in _targetObjectType.GetRuntimeMethods())
        {
            if (string.Equals(method.Name, MethodName, StringComparison.Ordinal)
                && (method.ReturnType == typeof(void) || method.ReturnType == typeof(Task))
                && method.IsPublic)
            {
                var parameters = method.GetParameters();
                if (parameters.Length == 0)
                {
                    // There can be only one parameterless method of the given name.
                    _cachedMethodDescriptor = new MethodDescriptor(method, parameters);
                }
                else if (parameters.Length == 2 && parameters[0].ParameterType == typeof(object))
                {
                    _methodDescriptors.Add(new MethodDescriptor(method, parameters));
                }
            }
        }

        // We didn't find a parameterless method, so we want to find a method that accepts null
        // as a second parameter, but if we have more than one of these it is ambiguous which
        // we should call, so we do nothing.
        if (_cachedMethodDescriptor is null)
        {
            foreach (var method in _methodDescriptors)
            {
                var typeInfo = method.SecondParameterTypeInfo;
                if (typeInfo is not null && (!typeInfo.IsValueType || typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    if (_cachedMethodDescriptor is not null)
                    {
                        _cachedMethodDescriptor = null;
                        return;
                    }

                    _cachedMethodDescriptor = method;
                }
            }
        }
    }

    [RequiresUnreferencedCode("This functionality is not compatible with trimming.")]
    [DebuggerDisplay($"{{{nameof(MethodInfo)}}}")]
    private class MethodDescriptor(MethodInfo methodInfo, ParameterInfo[] methodParameters)
    {
        public MethodInfo MethodInfo { get; private set; } = methodInfo;

        public ParameterInfo[] Parameters { get; private set; } = methodParameters;

        public int ParameterCount => Parameters.Length;

        public TypeInfo? SecondParameterTypeInfo
        {
            get => ParameterCount < 2 ? null : Parameters[1].ParameterType.GetTypeInfo();
        }
    }
}
