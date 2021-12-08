namespace DraggableDemo.Models;

public class Tile
{
    public string? Title { get; set; }

    public int Column { get; set; }

    public int Row { get; set; }

    public int ColumnSpan { get; set; }

    public int RowSpan { get; set; }

    public string? Background { get; set; }

    public override string? ToString() => Title;
}