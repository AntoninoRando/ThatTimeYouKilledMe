public abstract class CellType
{

}

public class Base : CellType
{

}

public class Wall : CellType
{

}

public class SpawnPoint : CellType
{
    public readonly PawnColor Color;

    public SpawnPoint(PawnColor color)
    {
        Color = color;
    }
}