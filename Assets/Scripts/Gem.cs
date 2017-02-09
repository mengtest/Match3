using System;
using UnityEngine;

/// <summary>
/// This class is used to describe a single gem on the board.
/// </summary>
public class Gem : MonoBehaviour
{
    public int Row { get; set; }
    public int Column { get; set; }
    public string Color { get; set; }

    /// <summary>
    /// Checks wheter or not this gem is of the same color as another gem.
    /// </summary>
    /// <param name="other">The gem to check against.</param>
    /// <returns>True, if this gem is the same type as the other gem.</returns>
    public bool IsSameColor(Gem other)
    {
        if (other == null || !(other is Gem))
            throw new ArgumentException("Supplied argument is not of type Gem.");

        return string.Compare(Color, other.Color, true) == 0;
    }

    /// <summary>
    /// Swaps the row and column values of two gems.
    /// </summary>
    /// <param name="gem1">The first gem to swap.</param>
    /// <param name="gem2">The second gem to swap.</param>
    public static void Swap(Gem gem1, Gem gem2)
    {
        int temp = gem1.Row;
        gem1.Row = gem2.Row;
        gem2.Row = temp;
        temp = gem1.Column;
        gem1.Column = gem2.Column;
        gem2.Column = temp;
    }
}