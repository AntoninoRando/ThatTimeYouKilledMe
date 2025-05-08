using System.Diagnostics;

/// <summary>
/// The action of killing an <i>alive</i> and <i>in-use</i> pawn.
/// </summary>
public class KillPawn : MatchAction
{
    #region INFO MESSAGES ------------------------------------------------------
    const string MSGAlreadyDead = "Can't kill a dead Pawn";
    const string MSGNotInUse = "Can't kill an unused Pawn";
    #endregion -----------------------------------------------------------------

    /// <summary>
    /// The pawn to kill.
    /// </summary>
    public Pawn Pawn;

    /// <summary>
    /// Why the pawn is dying.
    /// </summary>
    public string Reason;

    public KillPawn(Player actionAgent, Pawn pawnToKill, string killReason)
    {
        #region integrity checks -----------------------------------------------
        Debug.Assert(actionAgent != null);
        Debug.Assert(pawnToKill != null);
        Debug.Assert(killReason != null && killReason.Length > 0);
        #endregion -------------------------------------------------------------

        ActionAgent = actionAgent;
        Pawn = pawnToKill;
        Reason = killReason;
    }

    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        #region preconditions --------------------------------------------------
        if (!Pawn.Alive)
            return (ActionResolveFlag.ILLEGAL, MSGAlreadyDead);
        if (!Pawn.InUse)
            return (ActionResolveFlag.ILLEGAL, MSGNotInUse);
        #endregion -------------------------------------------------------------

        Pawn.Alive = false;
        Pawn.InUse = false;
        return (ActionResolveFlag.SUCCESS, "");
    }
}