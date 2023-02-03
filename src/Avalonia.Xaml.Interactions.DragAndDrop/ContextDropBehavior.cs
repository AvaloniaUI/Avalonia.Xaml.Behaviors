using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Xaml.Interactivity;

namespace Avalonia.Xaml.Interactions.DragAndDrop;

/// <summary>
/// 
/// </summary>
public class ContextDropBehavior : Behavior<Control>
{
    /// <summary>
    /// 
    /// </summary>
    public static string DataFormat = nameof(Context);

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<object?> ContextProperty =
        AvaloniaProperty.Register<ContextDropBehavior, object?>(nameof(Context));

    /// <summary>
    /// 
    /// </summary>
    public static readonly StyledProperty<IDropHandler?> HandlerProperty =
        AvaloniaProperty.Register<ContextDropBehavior, IDropHandler?>(nameof(Handler));

    /// <summary>
    /// 
    /// </summary>
    public object? Context
    {
        get => GetValue(ContextProperty);
        set => SetValue(ContextProperty, value);
    }

    /// <summary>
    /// 
    /// </summary>
    public IDropHandler? Handler
    {
        get => GetValue(HandlerProperty);
        set => SetValue(HandlerProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree()
    {
        if (AssociatedObject is { })
        {
            DragDrop.SetAllowDrop(AssociatedObject, true);
        }
        AssociatedObject?.AddHandler(DragDrop.DragEnterEvent, DragEnter);
        AssociatedObject?.AddHandler(DragDrop.DragLeaveEvent, DragLeave);
        AssociatedObject?.AddHandler(DragDrop.DragOverEvent, DragOver);
        AssociatedObject?.AddHandler(DragDrop.DropEvent, Drop);
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree()
    {
        if (AssociatedObject is { })
        {
            DragDrop.SetAllowDrop(AssociatedObject, false);
        }
        AssociatedObject?.RemoveHandler(DragDrop.DragEnterEvent, DragEnter);
        AssociatedObject?.RemoveHandler(DragDrop.DragLeaveEvent, DragLeave);
        AssociatedObject?.RemoveHandler(DragDrop.DragOverEvent, DragOver);
        AssociatedObject?.RemoveHandler(DragDrop.DropEvent, Drop);
    }

    private void DragEnter(object? sender, DragEventArgs e)
    {
        var sourceContext = e.Data.Get(ContextDropBehavior.DataFormat);
        var targetContext = Context ?? AssociatedObject?.DataContext;
        Handler?.Enter(sender, e, sourceContext, targetContext);
    }

    private void DragLeave(object? sender, RoutedEventArgs e)
    {
        Handler?.Leave(sender, e);
    }

    private void DragOver(object? sender, DragEventArgs e)
    {
        var sourceContext = e.Data.Get(ContextDropBehavior.DataFormat);
        var targetContext = Context ?? AssociatedObject?.DataContext;
        Handler?.Over(sender, e, sourceContext, targetContext);
    }

    private void Drop(object? sender, DragEventArgs e)
    {
        var sourceContext = e.Data.Get(ContextDropBehavior.DataFormat);
        var targetContext = Context ?? AssociatedObject?.DataContext;
        Handler?.Drop(sender, e, sourceContext, targetContext);
    }
}
