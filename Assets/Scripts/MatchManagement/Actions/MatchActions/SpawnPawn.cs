using System.Diagnostics;

/// <summary>
/// The action of spawning an <i>alive</i> pawn from a player's <i>reserve</i>.
/// This action may fail if there is no available pawn from the player's
/// reserve.
/// </summary>
public class SpawnPawn : MatchAction
{
    public Pawn Pawn;
    public Cell Cell;
    public string Reason;

    public SpawnPawn(Player actionAgent, Pawn pawn, Cell spawnCell, string reason)
    {
        #region integrity checks -----------------------------------------------
        Debug.Assert(actionAgent != null);
        Debug.Assert(pawn != null);
        Debug.Assert(spawnCell != null);
        Debug.Assert(reason != null && reason.Length > 0);
        #endregion -------------------------------------------------------------

        ActionAgent = actionAgent;
        Pawn = pawn;
        Cell = spawnCell;
        Reason = reason;
    }

    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        #region preconditions --------------------------------------------------
        if (Pawn.InUse)
            return (ActionResolveFlag.ILLEGAL, "Can't spawn an already in-use Pawn");
        if (!Pawn.Alive)
            return (ActionResolveFlag.ILLEGAL, "Can't spawn a dead Pawn");
        #endregion -------------------------------------------------------------

        // TODO add checks the spawn cell is not occupied.
        Pawn.InUse = true;
        return match.TakeAction(new ChangePawnCell(ActionAgent, Pawn, Cell));
    }
}