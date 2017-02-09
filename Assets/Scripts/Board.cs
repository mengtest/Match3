using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class represents a board of gems in the game.
/// </summary>
public class Board
{
    private GameObject[,] gems = new GameObject[Constants.Rows, Constants.Columns];
    private GameObject bG1;
    private GameObject bG2;

    /// <summary>
    /// The board can be accessed like an array.
    /// </summary>
    /// <param name="row">Row of the gem.</param>
    /// <param name="column">Column of the gem.</param>
    /// <returns>The gem at position [<paramref name="row"/>][<paramref name="column"/>]</returns>
    public GameObject this[int row, int column]
    {
        get
        {
            try
            {
                return gems[row, column];
            }
            catch (Exception ex) // Index out of range exception
            {
                throw ex;
            }
        }
        set
        {
            gems[row, column] = value;
        }
    }

    /// <summary>
    /// This method is to swap two GameObjects representing gems on the board.
    /// </summary>
    public void Swap(GameObject gem1, GameObject gem2)
    {
        // Save backups, in case we need to swap the gems back
        bG1 = gem1;
        bG2 = gem2;

        var g1 = gem1.GetComponent<Gem>();
        var g2 = gem2.GetComponent<Gem>();

        int g1Row = g1.Row;
        int g1Column = g1.Column;
        int g2Row = g2.Row;
        int g2Column = g2.Column;

        var temp = gems[g1Row, g1Column];
        gems[g1Row, g1Column] = gems[g2Row, g2Column];
        gems[g2Row, g2Column] = temp;

        Gem.Swap(g1, g2);
    }

    /// <summary>
    /// This methods reverts the last gem swap done.
    /// </summary>
    public void UndoSwap()
    {
        if (bG1 == null || bG2 == null)
            throw new Exception("No backup to undo with.");

        Swap(bG1, bG2);
    }

    /// <summary>
    /// Checks for a matching horizontal line of gems.
    /// </summary>
    /// <param name="gemGO">The gem that should be in the match.</param>
    /// <returns>True, if there are 3 or more gems that match each other, including the provided gem.</returns>
    private IEnumerable<GameObject> GetMatchesHorizontally(GameObject gemGO)
    {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(gemGO);
        var gem = gemGO.GetComponent<Gem>();
        // left side of the gem
        if (gem.Column != 0)
            for (int column = gem.Column - 1; column >= 0; column--)
            {
                if (gems[gem.Row, column].GetComponent<Gem>().IsSameColor(gem))
                {
                    matches.Add(gems[gem.Row, column]);
                }
                else
                    break; // we stop if we encounter another color
            }

        // right side of the gem
        if (gem.Column != Constants.Columns - 1)
            for (int column = gem.Column + 1; column < Constants.Columns; column++)
            {
                if (gems[gem.Row, column].GetComponent<Gem>().IsSameColor(gem))
                {
                    matches.Add(gems[gem.Row, column]);
                }
                else
                    break; // we stop if we encounter another color
            }

        if (matches.Count < 3)
            matches.Clear(); // found less than 2 matches with this gem, return empty list

        return matches.Distinct();
    }

    /// <summary>
    /// Checks for a matching vertical line of gems.
    /// </summary>
    /// <param name="gemGO">The gem that should be in the match.</param>
    /// <returns>True, if there are 3 or more gems that match each other, including the provided gem.</returns>
    private IEnumerable<GameObject> GetMatchesVertically(GameObject gemGO)
    {
        List<GameObject> matches = new List<GameObject>();
        matches.Add(gemGO);
        var gem = gemGO.GetComponent<Gem>();
        //check bottom
        if (gem.Row != 0)
            for (int row = gem.Row - 1; row >= 0; row--)
            {
                if (gems[row, gem.Column] != null &&
                    gems[row, gem.Column].GetComponent<Gem>().IsSameColor(gem))
                {
                    matches.Add(gems[row, gem.Column]);
                }
                else
                    break; // we stop if we encounter another color
            }

        //check top
        if (gem.Row != Constants.Rows - 1)
            for (int row = gem.Row + 1; row < Constants.Rows; row++)
            {
                if (gems[row, gem.Column] != null &&
                    gems[row, gem.Column].GetComponent<Gem>().IsSameColor(gem))
                {
                    matches.Add(gems[row, gem.Column]);
                }
                else
                    break; // we stop if we encounter another color
            }

        if (matches.Count < 3)
            matches.Clear();  // found less than 2 matches with this gem, return empty list

        return matches.Distinct();
    }

    /// <summary>
    /// Checks for a matching line of gems.
    /// </summary>
    /// <param name="gemGO">The gem that should be in the match.</param>
    /// <returns>True, if there are 3 or more gems that match each other in a line, including the provided gem.</returns>
    public IEnumerable<GameObject> GetMatches(GameObject gemGO)
    {
        var matches = new List<GameObject>();
        var hMatches = GetMatchesHorizontally(gemGO);
        var vMatches = GetMatchesVertically(gemGO);
        matches.AddRange(hMatches);
        matches.AddRange(vMatches);

        return matches.Distinct();
    }

    /// <summary>
    /// Removes a gem GameObject from the board (sets it to null on board).
    /// </summary>
    /// <param name="gemGO">The gem GameObject to remove.</param>
    public void Remove(GameObject gemGO)
    {
        gems[gemGO.GetComponent<Gem>().Row, gemGO.GetComponent<Gem>().Column] = null;
    }

    /// <summary>
    /// This method collapses the specified columns (moves the candy down over empty spaces).
    /// </summary>
    /// <param name="columns">The columns to collapse.</param>
    public void Collapse(IEnumerable<int> columns)
    {
        foreach (var column in columns)
        {
            for (int row = 0; row < Constants.Rows - 1; row++)
            {
                if (gems[row, column] == null) // Found an empty spot
                {
                    for (int row2 = row + 1; row2 < Constants.Rows; row2++) // find first non-null gem
                    {
                        if (gems[row2, column] != null)
                        {
                            // Move the gem down
                            gems[row, column] = gems[row2, column];
                            gems[row2, column] = null;
                            gems[row, column].GetComponent<Gem>().Row = row;
                            gems[row, column].GetComponent<Gem>().Column = column;
                            break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// This method returns a list of empty slots in a column on the board.
    /// </summary>
    /// <param name="column">The column to check for empty slots.</param>
    /// <returns>List of empty slots in a column.</returns>
    public IEnumerable<GemPosition> GetEmptySlotsInColumn(int column)
    {
        List<GemPosition> slots = new List<GemPosition>();
        for (int row = 0; row < Constants.Rows; row++)
        {
            if (gems[row, column] == null)
                slots.Add(new GemPosition() { Row = row, Column = column });
        }
        return slots;
    }

    /// <summary>
    /// This method returns the indices of columns that are not full.
    /// </summary>
    /// <returns>List of not entirely full columns.</returns>
    public IEnumerable<int> GetColumnsMissingGems()
    {
        List<int> columns = new List<int>();
        for (int column = 0; column < Constants.Columns; column++)
        {
            for (int row = Constants.Rows -1; row >= 0; row--)
            {
                if (gems[row, column] == null)
                {
                    columns.Add(column);
                    break;
                }
            }
        }
        return columns;
    }
}