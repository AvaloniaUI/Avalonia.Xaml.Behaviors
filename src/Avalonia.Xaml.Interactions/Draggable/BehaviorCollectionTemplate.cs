using System;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using Avalonia.Styling;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Draggable
{
    /// <summary>
    /// 
    /// </summary>
    public class BehaviorCollectionTemplate : ITemplate
    {
        /// <summary>
        /// 
        /// </summary>
        [Content]
        [TemplateContent(TemplateResultType = typeof(BehaviorCollection))]
        public object? Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        object? ITemplate.Build() => TemplateContent.Load<BehaviorCollection>(Content).Result;
    }
}
