using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Avalonia.Xaml.Interactions.Draggable;

/// <summary>
/// 
/// </summary>
public class SelectionAdorner : Control
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="context"></param>
    public override void Render(DrawingContext context)
    {
        var adornedElement = GetValue(AdornerLayer.AdornedElementProperty);
        if (adornedElement is null)
        {
            return;
        }
 
        var bounds = adornedElement.Bounds;
        var brush = new SolidColorBrush(Colors.White) { Opacity = 0.5 };
        var pen = new Pen(new SolidColorBrush(Colors.Black), 1.5);
        var r = 5.0;
        var topLeft = new RectangleGeometry(new Rect(-r, -r, r + r, r + r));
        var topRight = new RectangleGeometry(new Rect(-r, bounds.Height - r, r + r, r + r));
        var bottomLeft = new RectangleGeometry(new Rect(bounds.Width - r, -r, r + r, r + r));
        var bottomRight = new RectangleGeometry(new Rect(bounds.Width - r, bounds.Height - r, r + r, r + r));

        context.DrawGeometry(brush, pen, topLeft);
        context.DrawGeometry(brush, pen, topRight);
        context.DrawGeometry(brush, pen, bottomLeft);
        context.DrawGeometry(brush, pen, bottomRight);
    }
}