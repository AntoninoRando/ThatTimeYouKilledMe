using System.Diagnostics;

public class MoveFocus : MatchAction
{
    #region INFO MESSAGES ------------------------------------------------------
    const string MSGRoundEnd = "Wait until the end of the round";
    const string MSGSameTimeline = "New focus must be on a different timeline";
    const string MSGNoPawns = "There are no Pawns to move in that timeline";
    #endregion -----------------------------------------------------------------

    public Timeline Focus;

    public MoveFocus(Player actionAgent, Timeline focus)
    {
        #region integrity checks -----------------------------------------------
        Debug.Assert(actionAgent != null);
        #endregion -------------------------------------------------------------

        ActionAgent = actionAgent;
        Focus = focus;
    }

    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        #region preconditions --------------------------------------------------
        if (ActionAgent.ActionsPoint > 0)
            return (ActionResolveFlag.ILLEGAL, MSGRoundEnd);
        if (Focus == ActionAgent.Focus)
            return (ActionResolveFlag.ILLEGAL, MSGSameTimeline);
        if (!match.AnyPawnInTimeline(Focus, ActionAgent))
            return (ActionResolveFlag.ILLEGAL, MSGNoPawns);
        #endregion -------------------------------------------------------------

        ActionAgent.Focus = Focus;
        return (ActionResolveFlag.SUCCESS, "");
    }
}