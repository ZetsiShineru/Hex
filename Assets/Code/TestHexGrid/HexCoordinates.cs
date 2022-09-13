using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexCoordinates : MonoBehaviour
{
    public static float xOffset = 1, yOffset = 1, zOffset = 0.867f;

    internal Vector3Int GetHexCoords() => offsetCoordinates;

    [Header("Offset coordinates")]
    [SerializeField]
    private Vector3Int offsetCoordinates;

    private void Awake()
    {
        offsetCoordinates = ConvertPositionToOddset(transform.position);
    }

    Vector3Int ConvertPositionToOddset(Vector3 position)
    {
        int x = Mathf.CeilToInt(position.x / xOffset);
        int y = Mathf.RoundToInt(position.y / yOffset);
        int z = Mathf.RoundToInt(position.z / zOffset);

        return new Vector3Int(x, y, z);
    }
}
