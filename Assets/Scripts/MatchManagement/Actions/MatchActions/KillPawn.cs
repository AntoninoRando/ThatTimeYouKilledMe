using System.Diagnostics;

/// <summary>
/// The action of killing an <i>alive</i> and <i>in-use</i> pawn. Trying to
/// perform this action of a pawn that is either dead or not in-use will result
/// in an illegal move.
/// </summary>
public class KillPawn : MatchAction
{
    #region INFO MESSAGES ------------------------------------------------------
    const string MSGAlreadyDead = "Can't kill a dead Pawn";
    const string MSGNotInUse = "Can't kill an unused Pawn";
    #endregion -----------------------------------------------------------------



    #region FIELDS -------------------------------------------------------------
    /// <summary>
    /// The pawn to kill.
    /// </summary>
    public Pawn Pawn;

    /// <summary>
    /// Why the pawn is dying.
    /// </summary>
    public string Reason;
    #endregion -----------------------------------------------------------------



    #region CONSTRUCTORS -------------------------------------------------------
    public KillPawn(Player actionAgent, Pawn pawnToKill, string killReason)
    {
        #region integrity checks
        Debug.Assert(actionAgent != null);
        Debug.Assert(pawnToKill != null);
        Debug.Assert(killReason != null && killReason.Length > 0);
        #endregion


        ActionAgent = actionAgent;
        Pawn = pawnToKill;
        Reason = killReason;
    }
    #endregion -----------------------------------------------------------------



    #region MATCH-ACTION OVERRIDES ---------------------------------------------
    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        #region preconditions
        if (!Pawn.Alive) return (ActionResolveFlag.ILLEGAL, MSGAlreadyDead);
        if (!Pawn.InUse) return (ActionResolveFlag.ILLEGAL, MSGNotInUse);
        #endregion

        
        Pawn.Alive = false;
        Pawn.InUse = false;
        if (ActionAgent.PawnInUse == Pawn)
            ActionAgent.PawnInUse = null;
        return (ActionResolveFlag.SUCCESS, "");
    }
    #endregion -----------------------------------------------------------------

}