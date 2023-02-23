using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Unit : MonoBehaviour
{
    [SerializeField]
    private int movementPoints = 20;
    public int MovementPoints { get => movementPoints; }
    public int unit_Num = 1;

    [SerializeField]
    private float movementDuration = 1, rotationDuration = 0.3f;

    private GlowHighLight glowHighlight;
    private Queue<Vector3> pathPositions = new Queue<Vector3>();

    public event Action<Unit> MovementFinished;

    private void Awake()
    {
        glowHighlight = GetComponent<GlowHighLight>();
    }
    public void Deselect()
    {
        glowHighlight.ToggleGlow(false);
    }
    public void Select()
    {
        glowHighlight.ToggleGlow(false);
    }
    public void MoveThroughPath(List<Vector3> currentPath)
    {
        pathPositions = new Queue<Vector3>(currentPath);
        Vector3 firstTarget = pathPositions.Dequeue();
        StartCoroutine(RotationCoroutine(firstTarget, rotationDuration));
    }
    IEnumerator RotationCoroutine(Vector3 endPosition,float rotationDuration)
    {
        Quaternion startRotation = transform.rotation;
        endPosition.y = transform.position.y;
        Vector3 direction = endPosition - transform.position;
        Quaternion endRotation = Quaternion.LookRotation(direction, Vector3.up);

        if (Mathf.Approximately(Mathf.Abs(Quaternion.Dot(startRotation,endRotation)),1.0f) == false)//check that need to rotate?
        {
            float timeElapsed = 0;
            while (timeElapsed < rotationDuration)
            {
                timeElapsed += Time.deltaTime;
                float lerpStep = timeElapsed / rotationDuration; // 0-1
                transform.rotation = Quaternion.Lerp(startRotation, endRotation, lerpStep);
                yield return null;
            }
            transform.rotation = endRotation;
        }
        StartCoroutine(MovementCoroutine(endPosition));
    }
    IEnumerator MovementCoroutine(Vector3 endPosition)
    {

        Vector3 startPosition = transform.position;
        endPosition.y = startPosition.y;
        float timeElapsed = 0;

        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            float lerpStep = timeElapsed / movementDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, lerpStep);
            yield return null;
        }
        transform.position = endPosition;

        if (pathPositions.Count > 0)
        {
            Debug.Log("Selecting the next position!");
            StartCoroutine(RotationCoroutine(pathPositions.Dequeue(), rotationDuration));
        }
        else
        {
            Debug.Log("Movement finished!");
            MovementFinished?.Invoke(this);
            yield return new WaitForSeconds(1);
            if (this.gameObject.GetComponent<PlayerStats>().selectItem)
            {
                Debug.Log("SelecItem");
                GameObject.Find("GameController").GetComponent<Turn>().turn = TURNCount.TURN_SelectItem_Phase;
            }
            else 
            {
                Debug.Log("Battle");
                GameObject.Find("GameController").GetComponent<Turn>().turn = TURNCount.TURN_Battle_Phase;
            }
        }
    }
    public Vector3Int ConvertPositionToOffset(Vector3 position)
    {
        float xOffset = 1, zOffset = 0.867f;
        int x = Mathf.CeilToInt(position.x / xOffset);
        int z = Mathf.RoundToInt(position.z / zOffset);

        return new Vector3Int(x, 0, z);
    }
}
