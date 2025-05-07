using System.Diagnostics;
using System.Linq;

/// <summary>
/// The action of spawning an <i>alive</i> pawn from a player's <i>reserve</i>.
/// </summary>
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
        Debug.Assert(actionAgent != null);

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
        if (!match.Pawns.Any(x => PawnInTimeline(x, Focus, ActionAgent)))
            return (ActionResolveFlag.ILLEGAL, MSGNoPawns);
        #endregion -------------------------------------------------------------

        ActionAgent.Focus = Focus;
        return (ActionResolveFlag.SUCCESS, "");
    }

    bool PawnInTimeline(Pawn pawn, Timeline timeline, Player owner)
    {
        if (!pawn.Alive || !pawn.InUse) return false;
        if (pawn.Cell.Timeline != timeline) return false;
        if (pawn.Owner != owner) return false;
        return true;
    }
}