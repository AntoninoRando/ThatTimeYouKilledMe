using System;
using System.Linq;

/// <summary>
/// An handler of a single game match.
/// </summary>
public class Match
{
    public Player WhitePlayer;
    public Player BlackPlayer;
    public Player ActivePlayer;
    public readonly Map Map = new();
    public Pawn[] Pawns;

    #region EVENTS -------------------------------------------------------------

    public event Action<MatchAction> ActionStart;
    public event Action<MatchAction> ActionEnd;

    #endregion -----------------------------------------------------------------

    public void Initialize()
    {
        Initialize(new(), new());
    }

    public void Initialize(Player whitePlayer, Player blackPlayer)
    {
        WhitePlayer = whitePlayer;
        WhitePlayer.Focus = Timeline.PAST;
        BlackPlayer = blackPlayer;
        BlackPlayer.Focus = Timeline.FUTURE;
        ActivePlayer = WhitePlayer;
        Map.Make();
        Pawns = new Pawn[]
        {
            new(PawnColor.WHITE, WhitePlayer, true),
            new(PawnColor.WHITE, WhitePlayer, true),
            new(PawnColor.WHITE, WhitePlayer, true),
            new(PawnColor.WHITE, WhitePlayer),
            new(PawnColor.WHITE, WhitePlayer),
            new(PawnColor.WHITE, WhitePlayer),
            new(PawnColor.WHITE, WhitePlayer),
            new(PawnColor.BLACK, BlackPlayer),
            new(PawnColor.BLACK, BlackPlayer),
            new(PawnColor.BLACK, BlackPlayer),
            new(PawnColor.BLACK, BlackPlayer),
            new(PawnColor.BLACK, BlackPlayer, true),
            new(PawnColor.BLACK, BlackPlayer, true),
            new(PawnColor.BLACK, BlackPlayer, true),
        };
    }

    public (ActionResolveFlag, string) TakeAction(MatchAction action)
    {
        ActionStart?.Invoke(action);
        action.Resolve(this);
        ActionEnd?.Invoke(action);

        if (action.Flag == ActionResolveFlag.ILLEGAL)
        {
            return (action.Flag, action.Details);
        }
        if (action is MoveFocus)
        {
            // End turn
            ActivePlayer.PawnInUse = null;
            ActivePlayer.ActionsPoint = 2;
            ActivePlayer = ActivePlayer == WhitePlayer ? BlackPlayer : WhitePlayer;
        }
        return (action.Flag, action.Details);
    }

    /// <summary>
    /// Tries to take a pawn of a color from the reserve.
    /// </summary>
    /// <param name="color">The color of the pawn.</param>
    /// <returns>None if there is no available to-use pawn in the reserve. An
    /// option containing that pawn otherwise.</returns>
    public Option<Pawn> TakePawnFromReserve(PawnColor color)
    {
        var pawn = Pawns.FirstOrDefault(p => p.Color == color && !p.InUse && p.Alive);
        return pawn != null? Option<Pawn>.Some(pawn) : Option<Pawn>.None;
    }
}