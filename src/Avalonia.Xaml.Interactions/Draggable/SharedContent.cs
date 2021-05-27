using Avalonia.Controls;
using Avalonia.Metadata;

namespace Avalonia.Xaml.Interactions.Draggable
{
    /// <summary>
    /// 
    /// </summary>
    public class SharedContent : Control
    {
        /// <summary>
        /// 
        /// </summary>
        [Content]
        public object? Content { get; set; }
    }
}
