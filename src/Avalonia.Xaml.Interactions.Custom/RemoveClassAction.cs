using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Removes a specified <see cref="RemoveClassAction.ClassName"/> from <see cref="StyledElement.Classes"/> collection when invoked. 
/// </summary>
public class RemoveClassAction : AvaloniaObject, IAction
{
    /// <summary>
    /// Identifies the <seealso cref="ClassName"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<string> ClassNameProperty =
        AvaloniaProperty.Register<RemoveClassAction, string>(nameof(ClassName));

    /// <summary>
    /// Identifies the <seealso cref="StyledElement"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<StyledElement?> StyledElementProperty =
        AvaloniaProperty.Register<RemoveClassAction, StyledElement?>(nameof(StyledElement));

    /// <summary>
    /// Gets or sets the class name that should be removed. This is a avalonia property.
    /// </summary>
    public string ClassName
    {
        get => GetValue(ClassNameProperty);
        set => SetValue(ClassNameProperty, value);
    }

    /// <summary>
    /// Gets or sets the target styled element that class name that should be removed from. This is a avalonia property.
    /// </summary>
    [ResolveByName]
    public StyledElement? StyledElement
    {
        get => GetValue(StyledElementProperty);
        set => SetValue(StyledElementProperty, value);
    }
    /// <summary>
    /// Executes the action.
    /// </summary>
    /// <param name="sender">The <see cref="object"/> that is passed to the action by the behavior. Generally this is <seealso cref="IBehavior.AssociatedObject"/> or a target object.</param>
    /// <param name="parameter">The value of this parameter is determined by the caller.</param>
    /// <returns>True if the class is successfully added; else false.</returns>
    public object Execute(object? sender, object? parameter)
    {
        var target = GetValue(StyledElementProperty) is { } ? StyledElement : sender as StyledElement;
        if (target is null || string.IsNullOrEmpty(ClassName))
        {
            return false;
        }

        if (target.Classes.Contains(ClassName))
        {
            target.Classes.Remove(ClassName);
        }

        return true;
    }
}
