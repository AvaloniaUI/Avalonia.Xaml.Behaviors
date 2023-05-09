using Avalonia.Markup.Xaml.Templates;
using Avalonia.Metadata;
using Avalonia.Styling;

namespace Avalonia.Xaml.Interactivity;

/// <summary>
/// A template for creating a collection of <see cref="Behavior"/> objects.
/// </summary>
public class BehaviorCollectionTemplate : ITemplate
{
    /// <summary>
    /// Gets or sets the content of the template.
    /// </summary>
    [Content]
    [TemplateContent(TemplateResultType = typeof(BehaviorCollection))]
    public object? Content { get; set; }

    /// <summary>
    /// Builds the collection of behaviors from the template.
    /// </summary>
    /// <returns>The collection of behaviors created from the template.</returns>
    object? ITemplate.Build() => TemplateContent.Load<BehaviorCollection>(Content)?.Result;
}
