using System;

[Flags]
public enum ActionResolveFlag
{
    SUCCESS = 0,
    ILLEGAL = 1,
    ADVANCE_TURN = 2
}