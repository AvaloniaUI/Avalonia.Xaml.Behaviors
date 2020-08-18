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
        public static void RefreshDataBindingsOnActions(ActionCollection? actions)
        {
            if (actions == null)
            {
                return;
            }
            foreach (var action in actions)
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

                while (type != null && type != typeof(IAvaloniaObject))
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

                if (type != null)
                {
                    AvaloniaPropertyCache[type] = propertyList; 
                }
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
