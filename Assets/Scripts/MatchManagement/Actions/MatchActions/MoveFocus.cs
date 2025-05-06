using System.Diagnostics;
using System.Linq;

/// <summary>
/// The action of spawning an <i>alive</i> pawn from a player's <i>reserve</i>.
/// </summary>
public class MoveFocus : MatchAction
{
    public Timeline Focus;

    public MoveFocus(Player actionAgent, Timeline focus)
    {
        Debug.Assert(actionAgent != null);

        ActionAgent = actionAgent;
        Focus = focus;
    }

    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        if (ActionAgent.ActionsPoint > 0)
            return (ActionResolveFlag.ILLEGAL, "Can't move focus until round is end");
        if (Focus == ActionAgent.Focus)
            return (ActionResolveFlag.ILLEGAL, "Can't keep focus on the same timeline");
        if (match.Pawns.FirstOrDefault(p => p.Cell.Timeline == Focus && p.Owner == ActionAgent) == null)
            return (ActionResolveFlag.ILLEGAL, "Can't move focus to a timeline without pawns to use"); 

        ActionAgent.Focus = Focus;
        return (ActionResolveFlag.SUCCESS, "");
    }
}