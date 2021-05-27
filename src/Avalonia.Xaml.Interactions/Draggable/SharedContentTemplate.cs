using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;
using Avalonia.Styling;

namespace Avalonia.Xaml.Interactions.Draggable
{
    /// <summary>
    /// 
    /// </summary>
    public class SharedContentTemplate : ITemplate<SharedContent>
    {
        /// <summary>
        /// 
        /// </summary>
        [Content]
        [TemplateContent]
        public object? Content { get; set; }

        private static ControlTemplateResult Load(object templateContent)
        {
            if (templateContent is Func<IServiceProvider, object> direct)
            {
                return (ControlTemplateResult)direct(null!);
            }
            throw new ArgumentException(nameof(templateContent));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SharedContent Build()
        {
            return (SharedContent)Load(Content!).Control;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        object? ITemplate.Build() => Build().Content;
    }
}
