using System.Diagnostics;

/// <summary>
/// A <i>single</i> action (it cannot be repeated) by the rule that can be held 
/// during a match.
/// </summary>
public abstract class MatchAction
{
    /// <summary>
    /// Who is performing this action, or who performed an action that directly
    /// led to this action.
    /// </summary>
    public Player ActionAgent { get; set; }

    /// <summary>
    /// A description of how this action resulted (e.g. SUCCESS).
    /// </summary>
    public ActionResolveFlag Flag { get; private set; } = ActionResolveFlag.UNRESOLVED;

    /// <summary>
    /// Some additional details, to be contextualized with <c>Flag</c>, about 
    /// the result of this action resolution.
    /// </summary>
    public string Details { get; private set; } = "";

    /// <summary>
    /// Whether this action <c>Resolve</c> method has already been invoked.
    /// </summary>
    public bool Resolved { get; private set; } = false;

    /// <summary>
    /// What this action does.
    /// </summary>
    /// <param name="match">The match where this action takes place.</param>
    /// <returns></returns>
    protected abstract (ActionResolveFlag, string) ResolveEffect(Match match);

    /// <summary>
    /// Resolves this action in the given Match.
    /// </summary>
    /// <param name="match">The match where this action takes place.</param>
    public void Resolve(Match match)
    {
        Debug.Assert(!Resolved, "Can't resolve the same action twice; create " +
                                "a new one instead");
        (Flag, Details) = ResolveEffect(match);
        Resolved = true;
    }
}