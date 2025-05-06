using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    Map map;
    Match match;

    [Tooltip("A single cell prefab of the map.")]
    public CellObject cellObject;

    [Tooltip("A generic pawn prefab.")]
    public PawnObject pawnObjectPrefab;

    public MatchHandler MatchHandler;

    public readonly Dictionary<Cell, CellObject> CellObjects = new();

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

    }

    void UpdatePawnsObjectsPositions(MatchAction action)
    {
        if (action.Flag != ActionResolveFlag.SUCCESS) return;

        (var pawn, var cell) = action switch
        {
            MovePawn move => (move.PawnToMove, move.CellToReach),
            PushPawn push => (push.Pawn, push.Cell),
            SpawnPawn spawn => (spawn.Pawn, spawn.Cell),
            _ => (null, null)
        };
        if (pawn == null && cell == null) return;

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
            pawnObject = PawnObject.PawnObjects[pawn];
        var cellObj = CellObjects[cell];
        pawnObject.transform.SetParent(cellObj.transform);
        pawnObject.transform.localPosition = new(0, 0, pawnObject.transform.localPosition.z);
    }

    #endregion -----------------------------------------------------------------
}
