using System.Diagnostics;

public class ChangePawnCell : MatchAction
{
    public Pawn Pawn;
    public Cell Cell;

    public ChangePawnCell(Player actionAgent, Pawn pawn, Cell cell)
    {
        Debug.Assert(actionAgent != null);
        Debug.Assert(pawn != null);
        Debug.Assert(cell != null);

        ActionAgent = actionAgent;
        Pawn = pawn;
        Cell = cell;
    }

    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        Pawn.Cell = Cell;
        return (ActionResolveFlag.SUCCESS, "");
    }
}