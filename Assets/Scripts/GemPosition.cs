/// <summary>
/// This class represents the position of a gem.
/// </summary>
public class GemPosition
{
    public int Column { get; set; }
    public int Row { get; set; }

    public override string ToString()
    {
        return "[" + Row + "][" + Column + "]";
    }
}