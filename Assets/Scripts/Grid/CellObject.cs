using TMPro;
using UnityEngine;

public class CellObject : MonoBehaviour
{
    Cell cell;
    public Cell Cell
    {
        get => cell;
        set
        {
            cell = value;
            NumberText.text = value.Number >= 1 ? $"{value.Number}" : "";
        }
    }

    public TextMeshProUGUI NumberText;
}