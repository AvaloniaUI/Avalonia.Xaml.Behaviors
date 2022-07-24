using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using Avalonia.Styling;

namespace Avalonia.Xaml.Interactivity;

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
    object ITemplate.Build() => TemplateContent.Load<BehaviorCollection>(Content).Result;
}
