using System;

/// <summary>
/// A description of how a <c>MatchAction</c> resulted after being resolved.
/// </summary>
[Flags]
public enum ActionResolveFlag
{
    UNRESOLVED = 0,
    SUCCESS = 1,
    ILLEGAL = 2,
}