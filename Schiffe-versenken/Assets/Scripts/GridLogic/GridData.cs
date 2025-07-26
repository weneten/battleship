using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridData
{
    Dictionary<Vector3Int, PlacementData> placedObjects = new();

    public void AddObjectAt(Vector3Int gridPosition, Vector2Int objectSize, int id, int placedObjectIndex)
    {
        List<Vector3Int> possitionToOccupy = CalculatePositions(gridPosition, objectSize);
        PlacementData data = new PlacementData(possitionToOccupy, id, placedObjectIndex);

        //check if position is occupied (check if pos is in dict)
        foreach (var pos in possitionToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
            {
                throw new Exception($"Dictionary already contains this cell position {pos}");
            }
            placedObjects[pos] = data; //add PlacementData to pos of grid into dict
        }
    }

    private List<Vector3Int> CalculatePositions(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> returnVal = new();
        for (int x = 0; x < objectSize.x; x++)
        {
            for (int y = 0; y < objectSize.y; y++)
            {
                returnVal.Add(gridPosition + new Vector3Int(x, 0, y)); //add Vector to list with object size
            }
        }
        return returnVal;
    }

    public bool CanPlaceObjectAt(Vector3Int gridPosition, Vector2Int objectSize)
    {
        List<Vector3Int> positionsToOccupy = CalculatePositions(gridPosition, objectSize);
        //if obj is in vector pos return false else return true
        foreach (var pos in positionsToOccupy)
        {
            if (placedObjects.ContainsKey(pos))
                return false;
        }
        return true;
    }
}


public class PlacementData
{
    public List<Vector3Int> occupiedPositions; //reference type <- list off placed obj on grid
    public int ID { get; private set; } // save and loading data onto map / database
    public int PlacedObjectIndex { get; private set; } //use for a remove Object system (optional)

    public PlacementData(List<Vector3Int> occupiedPosition, int id, int placedObjectIndex)
    {
        this.occupiedPositions = occupiedPosition;
        ID = id;
        PlacedObjectIndex = placedObjectIndex;
    }
}
