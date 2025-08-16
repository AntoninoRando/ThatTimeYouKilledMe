using TMPro;
using UnityEngine;

public class InfoDisplay : MonoBehaviour
{
    public MatchHandler matchHandler;

    TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        matchHandler.Match.ActionEnd += UpdateText;
        UpdateText(null);
    }

    void UpdateText(MatchAction _)
    {
        string player = matchHandler.Match.ActivePlayer == matchHandler.Match.WhitePlayer
            ? "White"
            : "Black";
        text.text = $"Player: {player}" +
                    $"\nActions: {matchHandler.Match.ActivePlayer.ActionsPoint}" +
                    $"\nFocus: {matchHandler.Match.ActivePlayer.Focus}";
    }
}