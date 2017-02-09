using System;
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

    public static int HasAtLeastXPotentialMatches(Board board, uint minimumMatchesToStopEarly)
    {
        if (minimumMatchesToStopEarly > 10)
            throw new ArgumentException("minimumMatches must be at max 10.");
        int matchCount = 0;
        for (int row = 0; row < Constants.Rows; row++)
        {
            for (int column = 0; column < Constants.Columns; column++)
            {
                if (column <= Constants.Columns - 2)
                {
                    if (board[row, column].GetComponent<Gem>().
                IsSameColor(board[row, column + 1].GetComponent<Gem>()))
                    {
                        /* * * * *
                         * & & * *
                         & * * * * */
                        if (row >= 1 && column >= 1)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row - 1, column - 1].GetComponent<Gem>()))
                                matchCount++;

                        /* * * * *
                         & * * * *
                         * & & * */
                        if (row <= Constants.Rows - 2 && column >= 1)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row + 1, column - 1].GetComponent<Gem>()))
                                matchCount++;
                    }
                }

                if (column <= Constants.Columns - 3)
                {
                    if (board[row, column].GetComponent<Gem>().
                        IsSameColor(board[row, column + 1].GetComponent<Gem>()))
                    {
                        /* * * * *
                         * & & * *
                         * * * & */
                        if (row >= 1 && column <= Constants.Columns - 3)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row - 1, column + 2].GetComponent<Gem>()))
                                matchCount++;

                        /* * * & *
                         * & & * *
                         * * * * */
                        if (row <= Constants.Rows - 2 && column <= Constants.Columns - 3)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row + 1, column + 2].GetComponent<Gem>()))
                                matchCount++;
                    }

                    if (board[row, column].GetComponent<Gem>().
                        IsSameColor(board[row, column + 2].GetComponent<Gem>()))
                    {
                        /* * * * *
                         * & * & *
                         * * & * */
                        if (row >= 1 && column <= Constants.Columns - 3)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row - 1, column + 1].GetComponent<Gem>()))
                                matchCount++;

                        /* * & * *
                         * & * & *
                         * * * * */
                        if (row <= Constants.Rows - 2 && column <= Constants.Columns - 3)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row + 1, column + 1].GetComponent<Gem>()))
                                matchCount++;
                    }
                }

                if (matchCount >= minimumMatchesToStopEarly) return matchCount;

                if (column <= Constants.Columns - 4)
                {
                    /* * * * *
                     * & & * &
                     * * * * */
                    if (board[row, column].GetComponent<Gem>().
                       IsSameColor(board[row, column + 1].GetComponent<Gem>()) &&
                       board[row, column].GetComponent<Gem>().
                       IsSameColor(board[row, column + 3].GetComponent<Gem>()))
                        matchCount++;
                }

                /* * * * *
                 * & * & &
                 * * * * */
                if (column >= 2 && column <= Constants.Columns - 2)
                {
                    if (board[row, column].GetComponent<Gem>().
                       IsSameColor(board[row, column + 1].GetComponent<Gem>()) &&
                       board[row, column].GetComponent<Gem>().
                       IsSameColor(board[row, column - 2].GetComponent<Gem>()))
                        matchCount++;
                }

                if (matchCount >= minimumMatchesToStopEarly) return matchCount;

                if (row <= Constants.Rows - 2)
                {
                    if (board[row, column].GetComponent<Gem>().
                IsSameColor(board[row + 1, column].GetComponent<Gem>()))
                    {
                        /* * * * *
                         * & * * *
                         * & * * *
                         & * * * */
                        if (column >= 1 && row >= 1)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row - 1, column - 1].GetComponent<Gem>()))
                                matchCount++;

                        /* * * * *
                         * & * * *
                         * & * * *
                         * * & * */
                        if (column <= Constants.Columns - 2 && row >= 1)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row - 1, column + 1].GetComponent<Gem>()))
                                matchCount++;
                    }
                }

                if (matchCount >= minimumMatchesToStopEarly) return matchCount;

                if (row <= Constants.Rows - 3)
                {
                    if (board[row, column].GetComponent<Gem>().
                        IsSameColor(board[row + 1, column].GetComponent<Gem>()))
                    {
                        /* * * * *
                         & * * * *
                         * & * * *
                         * & * * */
                        if (column >= 1)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row + 2, column - 1].GetComponent<Gem>()))
                                matchCount++;

                        /* * * * *
                         * * & * *
                         * & * * *
                         * & * * */
                        if (column <= Constants.Columns - 2)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row + 2, column + 1].GetComponent<Gem>()))
                                matchCount++;
                    }

                    if (board[row, column].GetComponent<Gem>().
                    IsSameColor(board[row + 2, column].GetComponent<Gem>()))
                    {
                        /* * * * *
                         * & * * *
                         & * * * *
                         * & * * */
                        if (column >= 1)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row + 1, column - 1].GetComponent<Gem>()))
                                matchCount++;

                        /* * * * *
                         * & * * *
                         * * & * *
                         * & * * */
                        if (column <= Constants.Columns - 2)
                            if (board[row, column].GetComponent<Gem>().
                            IsSameColor(board[row + 1, column + 1].GetComponent<Gem>()))
                                matchCount++;
                    }
                }

                if (matchCount >= minimumMatchesToStopEarly) return matchCount;

                if (row <= Constants.Rows - 4)
                {
                    /* & * * *
                     * * * * *
                     * & * * *
                     * & * * */
                    if (board[row, column].GetComponent<Gem>().
                       IsSameColor(board[row + 1, column].GetComponent<Gem>()) &&
                       board[row, column].GetComponent<Gem>().
                       IsSameColor(board[row + 3, column].GetComponent<Gem>()))
                        matchCount++;
                }

                /* & * * *
                 * & * * *
                 * * * * *
                 * & * * */
                if (row >= 2 && row <= Constants.Rows - 2)
                {
                    if (board[row, column].GetComponent<Gem>().
                       IsSameColor(board[row + 1, column].GetComponent<Gem>()) &&
                       board[row, column].GetComponent<Gem>().
                       IsSameColor(board[row - 2, column].GetComponent<Gem>()))
                        matchCount++;
                }

                if (matchCount >= minimumMatchesToStopEarly) return matchCount;
            }
            if (matchCount >= minimumMatchesToStopEarly) return matchCount;
        }
        return matchCount;
    }
}