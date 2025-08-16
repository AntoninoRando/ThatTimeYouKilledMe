using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    Map map;
    Match match;

    #region EDITOR FIELDS ------------------------------------------------------

    [Tooltip("A single cell prefab of the map.")]
    public CellObject cellObject;

    [Tooltip("A generic pawn prefab.")]
    public PawnObject pawnObjectPrefab;

    [Tooltip("The handler for the match using the Map this MapMaker will " +
             "make.")]
    public MatchHandler MatchHandler;

    #endregion -----------------------------------------------------------------

    public readonly Dictionary<Cell, CellObject> CellObjects = new();

    GameObject whiteFocusIndicator;
    GameObject blackFocusIndicator;

    #region UNITY LIFECYCLE ----------------------------------------------------

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        match = MatchHandler.Match;
        map = match.Map;
        // TODO migliorare il linking alle azioni: 1) differenziare per azione;
        // 2) aggiungere poi l'unlink.
        match.ActionEnd += UpdatePawnsObjectsPositions;

        int whites = 0;
        int blacks = 0;

        for (int row = 0; row < map.RowsCount; row++)
        {
            for (int column = 0; column < map.ColumnsAtRow(0).Length; column++)
            {
                Vector3 spawnPosition = new(column * 1.1f, row * -1.1f, 0);
                var square = Instantiate(cellObject, spawnPosition, Quaternion.identity);
                var cell = map.CellAt(row, column);
                square.Cell = cell;
                CellObjects.Add(cell, square);
                if (cell.Type is Wall)
                {
                    square.GetComponent<Renderer>().material.color = new(230, 0, 0);
                }
                if (cell.Type is SpawnPoint cellSpawn)
                {
                    var pawnObject = Instantiate(pawnObjectPrefab, square.transform);
                    pawnObject.Match = match;
                    pawnObject.transform.localPosition = new(0, 0, 7);

                    if (cellSpawn.Color == PawnColor.WHITE)
                    {
                        pawnObject.Pawn = match.Pawns[whites];
                        whites++;
                    }
                    else
                    {
                        blacks++;
                        pawnObject.Pawn = match.Pawns[^blacks];
                    }
                    pawnObject.Pawn.Cell = cell;
                    pawnObject.SetColor(cellSpawn.Color);
                }
            }
        }

        CreateFocusIndicators();
    }

    void UpdatePawnsObjectsPositions(MatchAction action)
    {
        if (action.Flag != ActionResolveFlag.SUCCESS) return;

        (var pawn, var cell) = action switch
        {
            MovePawn move => (move.Pawn, move.Cell),
            PushPawn push => (push.Pawn, push.Cell),
            SpawnPawn spawn => (spawn.Pawn, spawn.Cell),
            KillPawn kill => (kill.Pawn, null),
            _ => (null, null)
        };
        if (pawn == null) return;

        PawnObject pawnObject;
        if (action is SpawnPawn)
        {
            pawnObject = Instantiate(pawnObjectPrefab);
            pawnObject.Match = match;
            pawnObject.Pawn = pawn;
            pawnObject.SetColor(pawn.Color);
            pawnObject.transform.localPosition = new(0, 0, 7);
        }
        else
        {
            pawnObject = PawnObject.PawnObjects[pawn];
        }

        if (!pawn.Alive)
        {
            if (pawnObject.gameObject.activeSelf)
                pawnObject.gameObject.SetActive(false);
            return;
        }

        var cellObj = CellObjects[cell];
        pawnObject.transform.SetParent(cellObj.transform);
        pawnObject.transform.localPosition = new(0, 0, pawnObject.transform.localPosition.z);
    }

    void CreateFocusIndicators()
    {
        whiteFocusIndicator = CreateIndicator("WhiteFocusIndicator", Color.white);
        blackFocusIndicator = CreateIndicator("BlackFocusIndicator", Color.black);
        match.ActionEnd += UpdateFocusIndicators;
        UpdateFocusIndicators(null);
    }

    GameObject CreateIndicator(string name, Color color)
    {
        var go = new GameObject(name);
        var sr = go.AddComponent<SpriteRenderer>();
        var texture = new Texture2D(32, 16);
        for (int x = 0; x < texture.width; x++)
            for (int y = 0; y < texture.height; y++)
                texture.SetPixel(x, y, color);
        texture.Apply();
        sr.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f), 16);
        sr.sortingOrder = 10;
        return go;
    }

    void UpdateFocusIndicators(MatchAction _)
    {
        if (whiteFocusIndicator == null || blackFocusIndicator == null) return;
        whiteFocusIndicator.transform.position = new(GetTimelineCenterX(match.WhitePlayer.Focus), 0.7f, -1);
        blackFocusIndicator.transform.position = new(GetTimelineCenterX(match.BlackPlayer.Focus), -6.2f, -1);
    }

    float GetTimelineCenterX(Timeline timeline)
    {
        int index = timeline switch
        {
            Timeline.PAST => 0,
            Timeline.PRESENT => 1,
            Timeline.FUTURE => 2,
            _ => 0
        };
        float centerColumn = 2.5f + index * 6f;
        return centerColumn * 1.1f;
    }

    #endregion -----------------------------------------------------------------
}
