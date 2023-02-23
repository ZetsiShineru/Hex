using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Direction
{
    public static List<Vector3Int> directionsOffsetOdd = new List<Vector3Int>
    {
        new Vector3Int(-1,0,1), //N1
        new Vector3Int(0,0,1), //N2
        new Vector3Int(1,0,0), //E
        new Vector3Int(0,0,-1), //S2
        new Vector3Int(-1,0,-1), //S1
        new Vector3Int(-1,0,0), //W
    };
    public static List<Vector3Int> directionsOffsetEven = new List<Vector3Int>
    {
        new Vector3Int(0,0,1), //N1
        new Vector3Int(1,0,1), //N2
        new Vector3Int(1,0,0), //E
        new Vector3Int(1,0,-1), //S2
        new Vector3Int(0,0,-1), //S1
        new Vector3Int(-1,0,0), //W
    };
    public static List<Vector3Int> GetDirectionList(int z)
        => z % 2 == 0 ? directionsOffsetEven : directionsOffsetOdd;
}

public class HexGrid : MonoBehaviour
{
    Dictionary<Vector3Int, Hex> hexTileDict = new Dictionary<Vector3Int, Hex>();
    Dictionary<Vector3Int, List<Vector3Int>> hexTileNeighboursDict = new Dictionary<Vector3Int, List<Vector3Int>>();

    List<Hex> hightLightHex = new List<Hex>();

    private void Start()
    {
        foreach(Hex hex in FindObjectsOfType<Hex>())
        {
            hexTileDict[hex.HexCoords] = hex;
        }
    }
    public Hex GetTileAt(Vector3Int hexCoordinates)
    {
        Hex result = null;
        hexTileDict.TryGetValue(hexCoordinates, out result);
        return result;
    }
    public List<Vector3Int> GetNeighboursFor(Vector3Int hexCoordinates)
    {
        if (hexTileDict.ContainsKey(hexCoordinates) == false)
            return new List<Vector3Int>();
        if (hexTileNeighboursDict.ContainsKey(hexCoordinates))
            return hexTileNeighboursDict[hexCoordinates];

        hexTileNeighboursDict.Add(hexCoordinates, new List<Vector3Int>());

        foreach (Vector3Int direction in Direction.GetDirectionList(hexCoordinates.z))
        {
            if(hexTileDict.ContainsKey(hexCoordinates + direction))
            {
                hexTileNeighboursDict[hexCoordinates].Add(hexCoordinates + direction);
            }
        }
        return hexTileNeighboursDict[hexCoordinates];
    }
    public Vector3Int GetClosetHex(Vector3 worldPosition)
    {
        worldPosition.y = 0;
        return HexCoordinates.ConvertPositionToOffset(worldPosition);
    }
    public void turnOnHighLight(int hexZone)
    {
        foreach (Hex hex in FindObjectsOfType<Hex>())
        {
            if(hex.CheckHexZone(hexZone)&&hex.CheckSpawnType())
            {
                hex.EnableHighlight();
                hex.HighlightPath();
                hightLightHex.Add(hex);
            }
        }
    }
    public void turnOffHighLight()
    {
        foreach (Hex hex in hightLightHex)
        {
            hex.ResetHighlight();
            hex.DisableHighlight();
        }
        hightLightHex.Clear();
    }
}
