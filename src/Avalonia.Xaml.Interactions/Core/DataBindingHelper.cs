using System;
using System.Collections.Generic;
using System.Reflection;
using Avalonia.Data;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Core
{
    internal static class DataBindingHelper
    {
        private static readonly Dictionary<Type, List<AvaloniaProperty>> _avaloniaPropertyCache = new();

        /// <summary>
        /// Ensures that all binding expression on actions are up to date.
        /// </summary>
        /// <remarks>
        /// <see cref="DataTriggerBehavior"/> fires during data binding phase. Since the <see cref="ActionCollection"/> is a child of the behavior,
        /// bindings on the action  may not be up-to-date. This routine is called before the action
        /// is executed in order to guarantee that all bindings are refreshed with the most current data.
        /// </remarks>
        public static void RefreshDataBindingsOnActions(ActionCollection? actions)
        {
            if (actions is null)
            {
                return;
            }

            foreach (var action in actions)
            {
                foreach (var property in GetAvaloniaProperties(action.GetType()))
                {
                    RefreshBinding(action, property);
                }
            }
        }

        private static IEnumerable<AvaloniaProperty> GetAvaloniaProperties(Type? type)
        {
            if (type is { })
            {
                if (_avaloniaPropertyCache.TryGetValue(type, out var propertyListCached))
                {
                    return propertyListCached;
                }
            }

            var propertyList = new List<AvaloniaProperty>();

            while (type is { } && type != typeof(IAvaloniaObject))
            {
                foreach (var fieldInfo in type.GetRuntimeFields())
                {
                    if (fieldInfo.IsPublic && fieldInfo.FieldType == typeof(AvaloniaProperty))
                    {
                        if (fieldInfo.GetValue(null) is AvaloniaProperty property)
                        {
                            propertyList.Add(property);
                        }
                    }
                }

                type = type.GetTypeInfo().BaseType;
            }

            if (type is { })
            {
                _avaloniaPropertyCache[type] = propertyList;
            }

            return propertyList;
        }

        private static void RefreshBinding(IAvaloniaObject target, AvaloniaProperty property)
        {
            if (target.GetValue(property) is IBinding binding)
            {
                target.Bind(property, binding);
            }
        }
    }
}
