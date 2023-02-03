using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.Custom;

/// <summary>
/// Adds a specified <see cref="AddClassAction.ClassName"/> to the <see cref="StyledElement.Classes"/> collection when invoked. 
/// </summary>
public class AddClassAction : AvaloniaObject, IAction
{
    /// <summary>
    /// Identifies the <seealso cref="ClassName"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<string> ClassNameProperty =
        AvaloniaProperty.Register<AddClassAction, string>(nameof(ClassName));

    /// <summary>
    /// Identifies the <seealso cref="StyledElement"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<StyledElement?> StyledElementProperty =
        AvaloniaProperty.Register<AddClassAction, StyledElement?>(nameof(StyledElement));

    /// <summary>
    /// Identifies the <seealso cref="RemoveIfExists"/> avalonia property.
    /// </summary>
    public static readonly StyledProperty<bool> RemoveIfExistsProperty =
        AvaloniaProperty.Register<AddClassAction, bool>(nameof(RemoveIfExists));

    /// <summary>
    /// Gets or sets the class name that should be added. This is a avalonia property.
    /// </summary>
    public string ClassName
    {
        get => GetValue(ClassNameProperty);
        set => SetValue(ClassNameProperty, value);
    }

    /// <summary>
    /// Gets or sets the target styled element that class name that should be added to. This is a avalonia property.
    /// </summary>
    [ResolveByName]
    public StyledElement? StyledElement
    {
        get => GetValue(StyledElementProperty);
        set => SetValue(StyledElementProperty, value);
    }

    /// <summary>
    /// Gets or sets the flag indicated whether to remove the class if already exists before adding. This is a avalonia property.
    /// </summary>
    public bool RemoveIfExists
    {
        get => GetValue(RemoveIfExistsProperty);
        set => SetValue(RemoveIfExistsProperty, value);
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

        if (RemoveIfExists && target.Classes.Contains(ClassName))
        {
            target.Classes.Remove(ClassName);
        }

        target.Classes.Add(ClassName);

        return true;
    }
}
