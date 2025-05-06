using System;
using UnityEngine;

public sealed class MatchHandler : MonoBehaviour
{
    public Match Match { get; private set; } = new();

    #region UNITY LIFECYCLE ----------------------------------------------------

    void Awake()
    {
        Match.Initialize();
    }

    #endregion -----------------------------------------------------------------

    public void ChangeTimeline(string timeline)
    {
        if (!Enum.TryParse(timeline, out Timeline timelineEnum))
        {
            Debug.LogError($"Invalid change timeline value: '{timeline}'");
            return;
        }
        (var result, var details) = Match.TakeAction(new MoveFocus(Match.ActivePlayer, timelineEnum));
        if (result != ActionResolveFlag.SUCCESS)
        {
            Debug.Log(details);
        }
    }
}