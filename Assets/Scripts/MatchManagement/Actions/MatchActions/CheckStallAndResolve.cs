using System.Diagnostics;

/// <summary>
/// The action of resolving a player stalled because they have still actions
/// left but nothing to do.
/// </summary>
public class CheckStallAndResolve : MatchAction
{
    #region INFO MESSAGES ------------------------------------------------------
    const string MSGNotTurn = "Not your turn";
    const string MSGNotStall = "No stall detected";
    #endregion -----------------------------------------------------------------



    #region CONSTRUCTORS -------------------------------------------------------
    public CheckStallAndResolve(Player actionAgent)
    {
        #region integrity checks
        Debug.Assert(actionAgent != null);
        #endregion


        ActionAgent = actionAgent;
    }
    #endregion -----------------------------------------------------------------



    #region MATCH-ACTION OVERRIDES ---------------------------------------------
    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        #region preconditions
        if (match.ActivePlayer != ActionAgent)
            return (ActionResolveFlag.ILLEGAL, MSGNotTurn);
        #endregion
        

        if (ActionAgent.ActionsPoint == 0)
            return (ActionResolveFlag.SUCCESS, MSGNotStall);
        if (ActionAgent.PawnInUse != null)
            return (ActionResolveFlag.SUCCESS, MSGNotStall);
        if (ActionAgent.ActionsPoint == 2 && match.AnyPawnInTimeline(ActionAgent.Focus, ActionAgent))
            return (ActionResolveFlag.SUCCESS, MSGNotStall);

        ActionAgent.PawnInUse = null;
        ActionAgent.ActionsPoint = 0;
        return (ActionResolveFlag.SUCCESS, "");
    }
    #endregion -----------------------------------------------------------------
}