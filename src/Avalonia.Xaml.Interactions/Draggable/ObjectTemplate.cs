using System;
using Avalonia.Metadata;
using Avalonia.Styling;

namespace Avalonia.Xaml.Interactions.Draggable
{
    // TODO: ObjectTemplate requires https://github.com/AvaloniaUI/Avalonia/pull/5468
    /*
    /// <summary>
    /// 
    /// </summary>
    public class ObjectTemplate : ITemplate
    {
        /// <summary>
        /// 
        /// </summary>
        [Content]
        [TemplateContent]
        public object? Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
