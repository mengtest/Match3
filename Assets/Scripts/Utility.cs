using UnityEngine;

/// <summary>
/// Contains various static helper methods, that perform calculations on the game data.
/// </summary>
public static class Utility
{
    /// <summary>
    /// Checks wheter two gems are next to each other vertically or horizontally.
    /// </summary>
    /// <param name="g1">First gem to check.</param>
    /// <param name="g2">Second gem to check.</param>
    /// <returns>True, if the provided gems are next to each other (not diagonally).</returns>
    public static bool AreNeighbors(Gem g1, Gem g2)
    {
        return (g1.Row == g2.Row || g1.Column == g2.Column) && Mathf.Abs(g1.Column - g2.Column) <= 1 && Mathf.Abs(g1.Row - g2.Row) <= 1;
    }
}
