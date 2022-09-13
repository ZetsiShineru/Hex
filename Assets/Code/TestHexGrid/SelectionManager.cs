using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    public LayerMask selectionMask;

    public HexGrid hexGrid;

    private List<Vector3Int> neighbours = new List<Vector3Int>();

    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
    }
    public void HAndleClick(Vector3 mousePos)
    {
        GameObject result;
        if(FindTarget(mousePos,out result))
        {
            Hex selectedHex = result.GetComponent<Hex>();

            selectedHex.DisableHighlight();
            foreach(Vector3Int neighbour in neighbours)
            {
                hexGrid.GetTileAt(neighbour).DisableHighlight();
            }

            neighbours = hexGrid.GetNeighboursFor(selectedHex.HexCoords);

            foreach (Vector3Int neighbour in neighbours)
            {
                hexGrid.GetTileAt(neighbour).EnableHighlight();
            }

            Debug.Log($"Neighbours for {selectedHex.HexCoords} are: ");
            foreach (Vector3Int neighbourPos in neighbours)
            {
                Debug.Log(neighbourPos);
            }
        }
    }
    private bool FindTarget(Vector3 mousePos,out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray,out hit,selectionMask))
        {
            result = hit.collider.gameObject;
            return true;
        }
        result = null;
        return false;
    }
}
