public class Map
{
    Cell[][] map;

    #region PROPERTIES ---------------------------------------------------------

    public Cell[] ColumnsAtRow(int row) => map[row];
    public Cell CellAt(int row, int column) => map[row][column];
    public int RowsCount => map.Length;

    #endregion -----------------------------------------------------------------

    public void Make()
    {
        map = new Cell[6][];

        // Make columns; make every cell
        for (int row = 0; row < 6; row++)
        {
            map[row] = new Cell[18];
            for (int col = 0; col < 18; col++)
            {
                Cell cell = new();
                map[row][col] = cell;
                cell.Cords = (col, row, 0);
            }
        }

        // make vertical walls
        for (int i = 1; i < 5; i++)
        {
            map[i][0].Assign(Timeline.PAST);
            map[i][5].Assign(Timeline.PAST);
            map[i][6].Assign(Timeline.PRESENT);
            map[i][11].Assign(Timeline.PRESENT);
            map[i][12].Assign(Timeline.FUTURE);
            map[i][17].Assign(Timeline.FUTURE);
        }

        // make horizontal walls
        for (int col = 1; col < 5; col++)
        {
            map[0][col].Assign(Timeline.PAST);
            map[5][col].Assign(Timeline.PAST);
        }
        for (int col = 7; col < 11; col++)
        {
            map[0][col].Assign(Timeline.PRESENT);
            map[5][col].Assign(Timeline.PRESENT);
        }
        for (int col = 13; col < 17; col++)
        {
            map[0][col].Assign(Timeline.FUTURE);
            map[5][col].Assign(Timeline.FUTURE);
        }

        // make other cells
        int cellNumber = 1;
        for (int row = 1; row <= 4; row++)
        {
            for (int col = 1; col <= 4; col++)
            {
                map[row][col].Assign(Timeline.PAST, cellNumber);
                map[row][col + 6].Assign(Timeline.PRESENT, cellNumber);
                map[row][col + 12].Assign(Timeline.FUTURE, cellNumber);
                cellNumber++;
            }
        }
    }
}
