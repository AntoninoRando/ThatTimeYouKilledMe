using System.Diagnostics;

public class KillPawn : MatchAction
{
    public Pawn PawnToKill;
    public string KillReason;

    public KillPawn(Player actionAgent, Pawn pawnToKill, string killReason)
    {
        Debug.Assert(actionAgent != null);
        Debug.Assert(pawnToKill != null);
        Debug.Assert(killReason != null);

        ActionAgent = actionAgent;
        PawnToKill = pawnToKill;
        KillReason = killReason;
    }

    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        if (!PawnToKill.Alive)
        {
            return (ActionResolveFlag.ILLEGAL, "Can't killed a died Pawn");
        }

        PawnToKill.Alive = false;
        PawnToKill.InUse = false;
        return (ActionResolveFlag.SUCCESS, "");
    }
}