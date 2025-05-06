using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(PawnMovement))]
[RequireComponent(typeof(IsAboveChecker))]
public class PawnObject : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Match Match;

    Pawn pawn;
    public Pawn Pawn
    {
        get => pawn;
        set
        {
            pawn = value;
            PawnObjects.Add(value, this);
        }
    }


    Vector3 pawnPosBeforeDrag;

    public static readonly Dictionary<Pawn, PawnObject> PawnObjects = new();

    #region UNITY LIFECYCLE ----------------------------------------------------
    #endregion -----------------------------------------------------------------
    #region PUBLIC METHODS -----------------------------------------------------

    public void SetColor(PawnColor color)
    {
        Pawn.Color = color;
        var rgb = color.RGB();
        Color unityColor = new(rgb.Item1, rgb.Item2, rgb.Item3);
        GetComponent<Renderer>().material.color = unityColor;
    }

    #endregion -----------------------------------------------------------------
    #region SELECTHANDLER overrides --------------------------------------------

    public void OnPointerDown(PointerEventData eventData)
    {
        pawnPosBeforeDrag = transform.position;
        var pawnMovement = GetComponent<PawnMovement>();

        if (Pawn.Owner != Match.ActivePlayer)
        {
            pawnMovement.enabled = false;
        }
        else
        {
            pawnMovement.enabled = true;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        var aboveChecker = GetComponent<IsAboveChecker>();
        if (aboveChecker.UnderObject == null)
        {
            transform.position = pawnPosBeforeDrag;
            return;
        }

        var cellObj = aboveChecker.UnderObject.GetComponent<CellObject>();
        (var returnFlag, var details) = Match.TakeAction(new MovePawn(Pawn.Owner, Pawn, cellObj.Cell));
        if (returnFlag != ActionResolveFlag.SUCCESS)
        {
            Debug.Log($"Movement failed: {details}");
            transform.position = pawnPosBeforeDrag;
        }
    }

    #endregion -----------------------------------------------------------------
}