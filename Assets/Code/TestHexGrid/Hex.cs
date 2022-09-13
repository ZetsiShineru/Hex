using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : MonoBehaviour
{
    [SerializeField]
    private GlowHighLight highlight;
    private HexCoordinates hexCoordinates;

    public Vector3Int HexCoords => hexCoordinates.GetHexCoords();

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
}
