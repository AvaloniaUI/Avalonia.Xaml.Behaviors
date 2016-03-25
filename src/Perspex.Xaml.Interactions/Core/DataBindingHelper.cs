// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reflection;
using Perspex.Data;
using Perspex.Xaml.Interactivity;

namespace Perspex.Xaml.Interactions.Core
{
    internal static class DataBindingHelper
    {
        private static readonly Dictionary<Type, List<PerspexProperty>> PerspexPropertyCache = new Dictionary<Type, List<PerspexProperty>>();

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
            foreach (PerspexObject action in actions)
            {
                foreach (PerspexProperty property in GetPerspexProperties(action.GetType()))
                {
                    RefreshBinding(action, property);
                }
            }
        }

        private static IEnumerable<PerspexProperty> GetPerspexProperties(Type type)
        {
            List<PerspexProperty> propertyList = null;

            if (!PerspexPropertyCache.TryGetValue(type, out propertyList))
            {
                propertyList = new List<PerspexProperty>();

                while (type != null && type != typeof(PerspexObject))
                {
                    foreach (FieldInfo fieldInfo in type.GetRuntimeFields())
                    {
                        if (fieldInfo.IsPublic && fieldInfo.FieldType == typeof(PerspexProperty))
                        {
                            PerspexProperty property = fieldInfo.GetValue(null) as PerspexProperty;
                            if (property != null)
                            {
                                propertyList.Add(property);
                            }
                        }
                    }

                    type = type.GetTypeInfo().BaseType;
                }

                PerspexPropertyCache[type] = propertyList;
            }

            return propertyList;
        }

        private static void RefreshBinding(PerspexObject target, PerspexProperty property)
        {
            IBinding binding = target.GetValue(property) as IBinding;
            if (binding != null)
            {
                target.Bind(property, binding);
            }
        }
    }
}
