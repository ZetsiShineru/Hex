using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HexType
{
    None,
    Default,
    Difficult,
    Road,
    Water,
    Obstacle
}

public class Hex : MonoBehaviour
{
    [SerializeField]
    private GlowHighLight highlight;
    private HexCoordinates hexCoordinates;

    public HexType hexType;
    [Range(1, 6)] public int hexZone;

    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

    public bool CheckSpawnType()
    {
        switch(hexType)
        {
            case HexType.Difficult:
                return true;
                break;
            case HexType.Default:
                return true;
                break;
            case HexType.Road:
                return true;
                break;
            default:
                return false;
                break;
        }
        return false;
    }
    public bool CheckHexZone(int diceInt)
    {
        if(diceInt == hexZone)
        {
            return true;
        }
        return false;
    }

    //public int GetCost()
    //    => hexType switch
    //    {
    //        HexType.Difficult => 20,
    //        HexType.Default => 10,
    //        HexType.Road => 5,
    //        _ => throw new Exception($"Hex of type {hexType} not supported")
    //    };
    public int GetCost()
    {
        switch(hexType)
        {
            case HexType.Difficult:
                return 20;
                break;
            case HexType.Default:
                return 10;
                break;
            case HexType.Road:
                return 5;
                break;
            default:
                Debug.Log($"Hex of type {hexType} not supported");
                return 0;
                break;
        }
        return 0;
    }
    public bool IsObstacle()
    {
        return this.hexType == HexType.Obstacle;
    }

    private void Awake()
    {
        hexCoordinates = GetComponent<HexCoordinates>();
        highlight = GetComponent<GlowHighLight>();
    }
    public void EnableHighlight()
    {
        highlight.ToggleGlow(true);
    }
    public void DisableHighlight()
    {
        highlight.ToggleGlow(false);
    }

    internal void ResetHighlight()
    {
        highlight.ResetGlowHighlight();
    }
    internal void HighlightPath()
    {
        highlight.HighlightValidPath();
    }
}
