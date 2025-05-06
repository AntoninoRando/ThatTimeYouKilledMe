public class Player
{
    public string Name;
    public int ActionsPoint = 2;
    public Pawn PawnInUse;

    /// <summary>
    /// The timeline in which the player must do its first action of the turn.
    /// </summary>
    public Timeline Focus;
}
