using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia.Xaml.Interactions.DragAndDrop;

/// <summary>
/// 
/// </summary>
public interface IDropHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="sourceContext"></param>
    /// <param name="targetContext"></param>
    void Enter(object? sender, DragEventArgs e, object? sourceContext, object? targetContext);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="sourceContext"></param>
    /// <param name="targetContext"></param>
    void Over(object? sender, DragEventArgs e, object? sourceContext, object? targetContext);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="sourceContext"></param>
    /// <param name="targetContext"></param>
    void Drop(object? sender, DragEventArgs e, object? sourceContext, object? targetContext);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Leave(object? sender, RoutedEventArgs e);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="sourceContext"></param>
    /// <param name="targetContext"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    bool Validate(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <param name="sourceContext"></param>
    /// <param name="targetContext"></param>
    /// <param name="state"></param>
    /// <returns></returns>
    bool Execute(object? sender, DragEventArgs e, object? sourceContext, object? targetContext, object? state);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void Cancel(object? sender, RoutedEventArgs e);
}