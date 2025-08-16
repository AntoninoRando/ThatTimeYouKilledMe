using System.Diagnostics;
using System.Linq;

/// <summary>
/// The action of moving a Pawn to a Cell based on all game rules and 
/// constraints.
/// </summary>
public class MovePawn : MatchAction
{
    #region INFO MESSAGES ------------------------------------------------------
    const string MSGDead = "Can't move a dead Pawn";
    const string MSGAnother = "Can't move another Pawn";
    const string MSGWall = "Can't move Pawn inside a Wall";
    const string MSGTooFar = "Can't move Pawn to a too far away cell";
    const string MSGNoFocus = "Can't start the turn moving a Pawn from a " +
                              "timeline with no focus";
    const string MSGTimelineOccupied = "Can't move Pawn to an another " +
                                       "timeline in an occupied cell";
    const string MSGAmbiguous = "Can't push another Pawn with an ambiguous " +
                                "direction";
    const string MSGSpawn = "Can't move back in time with no available new " +
                            "Pawn to spawn";
    #endregion -----------------------------------------------------------------



    #region FIELDS -------------------------------------------------------------
    public Pawn Pawn;
    public Cell Cell;
    #endregion -----------------------------------------------------------------



    #region CONSTRUCTORS -------------------------------------------------------
    public MovePawn(Player actionAgent, Pawn pawnToMove, Cell cellToReach)
    {
        #region integrity checks
        Debug.Assert(actionAgent != null);
        Debug.Assert(pawnToMove != null);
        Debug.Assert(cellToReach != null);
        #endregion


        ActionAgent = actionAgent;
        Pawn = pawnToMove;
        Cell = cellToReach;
    }
    #endregion -----------------------------------------------------------------



    #region MATCH-ACTION OVERRIDES ---------------------------------------------
    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        #region preconditions
        /*
            In this first section of the method, we only check if the movement
            can be completed. No object modification is performed, since in case
            of failure there should be no side-effects.
        */
        if (!Pawn.Alive) return (ActionResolveFlag.ILLEGAL, MSGDead);
        if (ActionAgent.PawnInUse != null && Pawn != ActionAgent.PawnInUse)
            return (ActionResolveFlag.ILLEGAL, MSGAnother);
        if (Cell.Type is Wall) return (ActionResolveFlag.ILLEGAL, MSGWall);

        (var actions, var cellDist, var timelineDist) = Pawn.Cell.ActionsTo(Cell);
        if (actions > ActionAgent.ActionsPoint)
            return (ActionResolveFlag.ILLEGAL, MSGTooFar);
        if (ActionAgent.ActionsPoint == 2 && Pawn.Cell.Timeline != ActionAgent.Focus)
            return (ActionResolveFlag.ILLEGAL,
                    $"{MSGNoFocus}; current focus: {ActionAgent.Focus}");

        var pawnIn = match.Pawns.FirstOrDefault(p => p.Cell == Cell);
        if (timelineDist > 0 && pawnIn != null)
            return (ActionResolveFlag.ILLEGAL, MSGTimelineOccupied);

        var pushPawnOpt = Option<(Pawn, Cell, int, int)>.None;
        if (pawnIn != null && pawnIn != Pawn)
        {
            // With more than 1 action we can not determine the push direction
            if (actions != 1) return (ActionResolveFlag.ILLEGAL, MSGAmbiguous);

            var col = Cell.Cords.Item1;
            var row = Cell.Cords.Item2;
            (var r, var c) = (Pawn.Cell.Number - Cell.Number) switch
            {
                -1 => (0, 1),
                1 => (0, -1),
                -4 => (1, 0),
                4 => (-1, 0),
                _ => (0, 0)
            };
            pushPawnOpt = Option<(Pawn, Cell, int, int)>.Some((pawnIn, match.Map.CellAt(row + r, col + c), r, c));
        }

        var spawnPawnOpt = Option<Pawn>.None;
        var spawnCellOpt = Option<Cell>.None;
        if (Cell.Timeline.IsBefore(Pawn.Cell.Timeline))
        {
            spawnPawnOpt = match.TakePawnFromReserve(Pawn.Color);
            if (!spawnPawnOpt.IsSome) return (ActionResolveFlag.ILLEGAL, MSGSpawn);
            spawnCellOpt = Option<Cell>.Some(Pawn.Cell);
        }
        #endregion


        /*
            If this final part of the function is reached, all checks have been
            passed and thus the movement can be done. Now the states of the
            objects can be altered.
        */

        ActionAgent.ActionsPoint -= actions;
        ActionAgent.PawnInUse = Pawn;
        match.TakeAction(new ChangePawnCell(ActionAgent, Pawn, Cell));
        if (spawnPawnOpt.TryUnwrap(out var spawnPawn))
        {
            match.TakeAction(new SpawnPawn(ActionAgent, spawnPawn, spawnCellOpt.Unwrap(), "BACK-IN-TIME MOVEMENT"));
        }
        if (pushPawnOpt.TryUnwrap(out var pushTuple))
        {
            (var pushPawn, var pushCell, var pushR, var pushC) = pushTuple;
            if (pushPawn.Color == Pawn.Color)
            {
                match.TakeAction(new KillPawn(ActionAgent, Pawn, "TIME PARADOX"));
                match.TakeAction(new KillPawn(ActionAgent, pushPawn, "TIME PARADOX"));
                return (ActionResolveFlag.SUCCESS, "");
            }
            match.TakeAction(new PushPawn(ActionAgent, pushPawn, pushCell, pushR, pushC));
        }
        return (ActionResolveFlag.SUCCESS, "");
    }
    #endregion -----------------------------------------------------------------
}