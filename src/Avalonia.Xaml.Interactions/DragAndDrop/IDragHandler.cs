using Avalonia.Input;

namespace Avalonia.Xaml.Interactions.DragAndDrop
{
    public interface IDragHandler
    {
        void BeforeDragDrop(object? sender, PointerEventArgs e, object? context);

        void AfterDragDrop(object? sender, PointerEventArgs e, object? context);
    }
}
