/// <summary>
/// An agent of the game who takes action (in the form of <c>MatchAction</c>)
/// during a match.
/// </summary>
public class Player
{
    /// <summary>
    /// The name of the player.
    /// </summary>
    public string Name;

    /// <summary>
    /// The amount of actions this player needs to do in its turn.
    /// </summary>
    public int ActionsPoint = 2;

    /// <summary>
    /// The pawn this player is currently using during its turn.
    /// </summary>
    public Pawn PawnInUse;

    /// <summary>
    /// The timeline in which the player must do its first action of the turn.
    /// </summary>
    public Timeline Focus;
}
