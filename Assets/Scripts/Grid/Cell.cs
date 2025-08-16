using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class Cell
{
    #region FIELDS -------------------------------------------------------------
    Timeline timeline;
    public Timeline Timeline => timeline;

    int number;
    public int Number => number;

    CellType type;
    public CellType Type => type;

    public (int, int, int) Cords;
    public float Width = 50;
    public float Height = 50;

    static readonly List<Cell> allCells = new();
    public static List<Cell> All => allCells;
    #endregion -----------------------------------------------------------------
    


    #region CONSTRUCTOR --------------------------------------------------------
    public Cell()
    {
        allCells.Add(this);
    }
    #endregion -----------------------------------------------------------------



    #region PUBLIC METHODS -----------------------------------------------------
    /// <summary>
    /// Gives a timeline and number to this cell. Use this method for cells that
    /// aren't walls.
    /// </summary>
    /// <param name="timeline"></param>
    /// <param name="number"></param>
    public void Assign(Timeline timeline, int number)
    {
        Debug.Assert(number > 0 && number < 17);
        this.timeline = timeline;
        this.number = number;
        type = number switch
        {
            1 => new SpawnPoint(PawnColor.WHITE),
            16 => new SpawnPoint(PawnColor.BLACK),
            _ => new Base()
        };
    }

    /// <summary>
    /// Gives a timeline to this cell. Use this method for cells that are walls.
    /// </summary>
    /// <param name="timeline"></param>
    public void Assign(Timeline timeline)
    {
        this.timeline = timeline;
        number = -1;
        type = new Wall();
    }

    /// <summary>
    /// Counts the number of action required to a pawn to move from this cell
    /// to <c>otherCell</c>.
    /// </summary>
    /// <param name="otherCell">The cell to reach.</param>
    /// <returns>Number of actions required to move from this to 
    /// <c>otherCell</c>.</returns>
    public (int, int, int) ActionsTo(Cell otherCell)
    {
        var timelineDistance = Math.Abs(otherCell.Timeline - Timeline);

        // Convert cell number to row and column (0-based index)
        int startRow = (number - 1) / 4;
        int startCol = (number - 1) % 4;
        int targetRow = (otherCell.Number - 1) / 4;
        int targetCol = (otherCell.Number - 1) % 4;

        // Calculate the Manhattan distance
        int rowDifference = Math.Abs(targetRow - startRow);
        int colDifference = Math.Abs(targetCol - startCol);

        var cellDistance = rowDifference + colDifference;

        return (cellDistance + timelineDistance, cellDistance, timelineDistance);
    }
    #endregion -----------------------------------------------------------------
}
