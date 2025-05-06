public enum PawnColor
{
    WHITE, BLACK, UNSET
}

public static class PawnColorMethods
{
    public static (int, int, int) RGB(this PawnColor color)
    {
        return color switch
        {
            PawnColor.WHITE => (255, 255, 255),
            PawnColor.BLACK => (0, 0, 0),
            _ => (255, 50, 50)
        };
    }
}