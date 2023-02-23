using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    public LayerMask selectionMask;

    public UnityEvent<GameObject> OnUnitSelected;
    public UnityEvent<GameObject> TerrainSelected;

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }
    public void HandleClick(Vector3 mousePos)
    {
        GameObject result;
        if(FindTarget(mousePos,out result))
        {
            if (UnitSelected(result))
            {
                OnUnitSelected?.Invoke(result);
            }
            else
            {
                TerrainSelected?.Invoke(result);
            }
            /*Hex selectedHex = result.GetComponent<Hex>();

            selectedHex.DisableHighlight();
            foreach(Vector3Int neighbour in neighbours)
            {
                hexGrid.GetTileAt(neighbour).DisableHighlight();
            }

            //neighbours = hexGrid.GetNeighboursFor(selectedHex.HexCoords);
            BFSResult bFSResult = GraphSearch.BFSGetRange(hexGrid, selectedHex.HexCoords,movementRange);
            neighbours = new List<Vector3Int>(bFSResult.GetRangePositions());

            foreach (Vector3Int neighbour in neighbours)
            {
                hexGrid.GetTileAt(neighbour).EnableHighlight();
            }

            Debug.Log($"Neighbours for {selectedHex.HexCoords} are: ");
            foreach (Vector3Int neighbourPos in neighbours)
            {
                Debug.Log(neighbourPos);
            }*/
        }
    }

    private bool UnitSelected(GameObject result)
    {
        return result.GetComponent<Unit>() != null && result.GetComponent<Unit>().unit_Num == GameObject.Find("GameController").GetComponent<Turn>().player_Turn;
    }

    private bool FindTarget(Vector3 mousePos,out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray,out hit,selectionMask))
        {
            result = hit.collider.gameObject;
            Debug.Log("T");
            Debug.Log(result.name);
            return true;
        }
        result = null;
        Debug.Log("F");
        return false;
    }
}
