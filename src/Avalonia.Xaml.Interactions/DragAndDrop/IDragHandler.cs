using Avalonia.Input;

namespace Avalonia.Xaml.Interactions.DragAndDrop;

/// <summary>
/// 
/// </summary>
public interface IDragHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="context"></param>
    void BeforeDragDrop(object? sender, PointerEventArgs e, object? context);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="context"></param>
    void AfterDragDrop(object? sender, PointerEventArgs e, object? context);
}