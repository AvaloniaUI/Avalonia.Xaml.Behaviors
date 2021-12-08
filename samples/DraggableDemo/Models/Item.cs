namespace DraggableDemo.Models;

public class Item
{
    public string? Title { get; set; }

    public double X { get; set; }

    public double Y { get; set; }

    public override string? ToString() => Title;
}