// Copyright (c) Wiesław Šoltés. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Core
{
    /// <summary>
    /// Focuses the associated control when executed.
    /// </summary>
    public sealed class FocusControlAction : AvaloniaObject, IAction
    {
        /// <inheritdoc/>
        public object Execute(object sender, object parameter)
        {
            (sender as Control)?.Focus();
            return null;
        }
    }
}
