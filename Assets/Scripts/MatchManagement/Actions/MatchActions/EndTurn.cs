using System.Diagnostics;

/// <summary>
/// The action of ending a Player's turn. If this action
/// </summary>
public class EndTurn : MatchAction
{
    #region INFO MESSAGES ------------------------------------------------------
    const string MSGNotTurn = "Not your turn";
    #endregion -----------------------------------------------------------------



    #region CONSTRUCTORS -------------------------------------------------------
    public EndTurn(Player actionAgent)
    {
        #region integrity checks
        Debug.Assert(actionAgent != null);
        #endregion


        ActionAgent = actionAgent;
    }
    #endregion -----------------------------------------------------------------



    #region MATCH-ACTION OVERRIDES ---------------------------------------------
    protected override (ActionResolveFlag, string) ResolveEffect(Match match)
    {
        #region preconditions
        if (match.ActivePlayer != ActionAgent)
            return (ActionResolveFlag.ILLEGAL, MSGNotTurn);
        #endregion

        
        ActionAgent.PawnInUse = null;
        ActionAgent.ActionsPoint = 2;
        match.ActivePlayer = ActionAgent == match.WhitePlayer ?
                             match.BlackPlayer : match.WhitePlayer;
        return (ActionResolveFlag.SUCCESS, "");
    }
    #endregion -----------------------------------------------------------------
}