using System.Diagnostics;
using System.Linq;

/// <summary>
/// The action of spawning an <i>alive</i> pawn from a player's <i>reserve</i>.
/// This action may fail if there is no available pawn from the player's
/// reserve.
/// </summary>
public class SpawnPawn : MatchAction
{
    #region INFO MESSAGES ------------------------------------------------------
    const string MSGAlreadyInUse = "Can't spawn an already in-use Pawn";
    const string MSGDead = "Can't spawn a dead Pawn";
    const string MSGOccupied = "Can't spawn a Pawn in an occupied cell";
    #endregion -----------------------------------------------------------------



    #region FIELDS -------------------------------------------------------------
    public Pawn Pawn;
    public Cell Cell;
    public string Reason;
    #endregion -----------------------------------------------------------------



    #region CONSTRUCTORS -------------------------------------------------------
    public SpawnPawn(Player actionAgent, Pawn pawn, Cell spawnCell, string reason)
    {
        #region integrity checks
        Debug.Assert(actionAgent != null);
        Debug.Assert(pawn != null);
        Debug.Assert(spawnCell != null);
        Debug.Assert(reason != null && reason.Length > 0);
        #endregion 

        ActionAgent = actionAgent;
        Pawn = pawn;
        Cell = spawnCell;
        Reason = reason;
    }
    #endregion -----------------------------------------------------------------



    #region MATCH-ACTION OVERRIDES ---------------------------------------------
    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        #region preconditions
        if (Pawn.InUse) return (ActionResolveFlag.ILLEGAL, MSGAlreadyInUse);
        if (!Pawn.Alive) return (ActionResolveFlag.ILLEGAL, MSGDead);
        if (match.Pawns.Any(x => x.Cell == Cell))
            return (ActionResolveFlag.ILLEGAL, MSGOccupied);
        #endregion

        
        Pawn.InUse = true;
        return match.TakeAction(new ChangePawnCell(ActionAgent, Pawn, Cell));
    }
    #endregion -----------------------------------------------------------------
}