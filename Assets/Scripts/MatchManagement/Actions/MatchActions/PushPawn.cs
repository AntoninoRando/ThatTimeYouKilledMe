using System.Diagnostics;
using System.Linq;

/// <summary>
/// Pushes a pawn into a cell or wall. In the latter case, the pawn is killed.
/// If the cell contains another pawn, it is pushed in the same direction. If
/// the other pawn is of the same color, both pawns are killed due to a time
/// paradox.
/// </summary>
public class PushPawn : MatchAction
{
    #region INFO MESSAGES ------------------------------------------------------
    const string MSGNoTimelinePush = "Can't push a Pawn into another timeline";
    #endregion -----------------------------------------------------------------



    #region FIELDS -------------------------------------------------------------
    public Pawn Pawn;
    public Cell Cell;
    public int RowDirection;
    public int ColumnDirection;
    #endregion -----------------------------------------------------------------



    #region CONSTRUCTORS -------------------------------------------------------
    public PushPawn(Player actionAgent, Pawn pawn, Cell cell, int rowDirection, int columnDirection)
    {
        #region integrity checks
        Debug.Assert(actionAgent != null);
        Debug.Assert(pawn != null);
        Debug.Assert(cell != null);
        #endregion


        ActionAgent = actionAgent;
        Pawn = pawn;
        Cell = cell;
        RowDirection = rowDirection;
        ColumnDirection = columnDirection;
    }
    #endregion -----------------------------------------------------------------



    #region MATCH-ACTION OVERRIDES ---------------------------------------------
    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        #region preconditions
        if (Cell.Timeline != Pawn.Cell.Timeline)
            return (ActionResolveFlag.ILLEGAL, MSGNoTimelinePush);
        #endregion

        
        if (Cell.Type is Wall)
        {
            match.TakeAction(new KillPawn(ActionAgent, Pawn, "SQUISH"));
            return (ActionResolveFlag.SUCCESS, "");
        }

        var pawnIn = match.Pawns.FirstOrDefault(p => p.Cell == Cell);
        if (pawnIn != null && pawnIn != Pawn)
        {
            if (pawnIn.Color == Pawn.Color)
            {
                match.TakeAction(new KillPawn(ActionAgent, Pawn, "TIME PARADOX"));
                match.TakeAction(new KillPawn(ActionAgent, pawnIn, "TIME PARADOX"));
                return (ActionResolveFlag.SUCCESS, "Players' pawns killed due to a time paradox");
            }

            var col = Cell.Cords.Item1 + ColumnDirection;
            var row = Cell.Cords.Item2 + RowDirection;
            match.TakeAction(new PushPawn(
                ActionAgent,
                pawnIn,
                match.Map.CellAt(row, col),
                RowDirection,
                ColumnDirection)
            );
        }

        match.TakeAction(new ChangePawnCell(ActionAgent, Pawn, Cell));
        return (ActionResolveFlag.SUCCESS, "");
    }
    #endregion -----------------------------------------------------------------
}