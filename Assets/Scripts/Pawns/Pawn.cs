public class Pawn
{
    public bool Alive = true;
    public bool InUse;

    public Cell Cell;
    public PawnColor Color;
    public Player Owner;

    #region CONSTRUCTORS -------------------------------------------------------

    public Pawn(PawnColor color, Player owner, bool inUse = false)
    {
        Color = color;
        Owner = owner;
        InUse = inUse;
    }

    #endregion -----------------------------------------------------------------
}
