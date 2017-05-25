// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Avalonia.Data;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Core
{
    internal static class DataBindingHelper
    {
        private static readonly Dictionary<Type, List<AvaloniaProperty>> AvaloniaPropertyCache = new Dictionary<Type, List<AvaloniaProperty>>();

        /// <summary>
        /// Ensures that all binding expression on actions are up to date.
        /// </summary>
        /// <remarks>
        /// <see cref="DataTriggerBehavior"/> fires during data binding phase. Since the <see cref="ActionCollection"/> is a child of the behavior,
        /// bindings on the action  may not be up-to-date. This routine is called before the action
        /// is executed in order to guarantee that all bindings are refreshed with the most current data.
        /// </remarks>
        public static void RefreshDataBindingsOnActions(ActionCollection actions)
        {
            foreach (AvaloniaObject action in actions)
            {
                foreach (AvaloniaProperty property in GetAvaloniaProperties(action.GetType()))
                {
                    RefreshBinding(action, property);
                }
            }
        }

        private static IEnumerable<AvaloniaProperty> GetAvaloniaProperties(Type type)
        {
            if (!AvaloniaPropertyCache.TryGetValue(type, out List<AvaloniaProperty> propertyList))
            {
                propertyList = new List<AvaloniaProperty>();

                while (type != null && type != typeof(AvaloniaObject))
                {
                    foreach (FieldInfo fieldInfo in type.GetRuntimeFields())
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

                AvaloniaPropertyCache[type] = propertyList;
            }

            return propertyList;
        }

        private static void RefreshBinding(AvaloniaObject target, AvaloniaProperty property)
        {
            if (target.GetValue(property) is IBinding binding)
            {
                target.Bind(property, binding);
            }
        }
    }
}
