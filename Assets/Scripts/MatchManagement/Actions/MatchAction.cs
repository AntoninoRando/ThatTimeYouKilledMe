public abstract class MatchAction
{
    public Player ActionAgent;
    public ActionResolveFlag Flag;
    public string Details;
    public bool Resolved;

    protected abstract (ActionResolveFlag, string) ResolveEffect(Match match);

    public void Resolve(Match match)
    {
        (Flag, Details) = ResolveEffect(match);
    }
}