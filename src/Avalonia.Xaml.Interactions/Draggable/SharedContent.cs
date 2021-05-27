using Avalonia.Controls;
using Avalonia.Metadata;

namespace Avalonia.Xaml.Interactions.Draggable
{
    public class SharedContent : Control
    {
        [Content]
        public object? Content { get; set; }
    }
}
