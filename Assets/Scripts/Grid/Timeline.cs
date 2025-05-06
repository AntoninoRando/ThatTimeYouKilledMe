public enum Timeline
{
    PAST=1, PRESENT=2, FUTURE=3
}

public static class TimelineMethods
{
    public static Option<Timeline> Next(this Timeline timeline)
    {
        return timeline switch
        {
            Timeline.PAST => Option<Timeline>.Some(Timeline.PRESENT),
            Timeline.PRESENT => Option<Timeline>.Some(Timeline.FUTURE),
            Timeline.FUTURE => Option<Timeline>.None,
            _ => throw new System.NotImplementedException()
        };
    }

    public static Option<Timeline> Previous(this Timeline timeline)
    {
        return timeline switch
        {
            Timeline.PAST => Option<Timeline>.None,
            Timeline.PRESENT => Option<Timeline>.Some(Timeline.PAST),
            Timeline.FUTURE => Option<Timeline>.Some(Timeline.PRESENT),
            _ => throw new System.NotImplementedException()
        };
    }

    public static bool IsBefore(this Timeline timeline, Timeline other)
    {
        return (int)timeline < (int)other;
    }

    public static bool IsAfter(this Timeline timeline, Timeline other)
    {
        return (int)timeline > (int)other;
    }
}