using System;
using Avalonia.Metadata;
using Avalonia.Styling;

namespace Avalonia.Xaml.Interactions.Draggable
{
    /*
    // TODO: ObjectTemplate requires https://github.com/AvaloniaUI/Avalonia/pull/5468
    public class ObjectTemplate : ITemplate
    {
        [Content]
        [TemplateContent]
        public object? Content { get; set; }

        object? ITemplate.Build()
        {
            if (Content is Func<IServiceProvider, object> direct)
            {
                return direct(null!);
            }
            throw new ArgumentException(nameof(Content));
        }
    }
    */
}
