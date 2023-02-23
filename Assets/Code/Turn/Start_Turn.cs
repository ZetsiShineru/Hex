using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum DiceColor
{
    red,
    green,
    blue,
    yellow,
    white,
    black
}
public class Start_Turn : MonoBehaviour
{
    public bool rollDice = true;
    private float delay = 5;

    [Range(2, 8)]
    public int howMuchPlayer;

    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private float diceScale;
    private Dice dice;

    public DiceColor diceColor;
    public UnityEvent cameraInvoke;

    private void Start()
    {
        dice = this.gameObject.GetComponent<Dice>();
    }
    public void Prepare_Start()
    {
        if(howMuchPlayer < 1)
        {
            rollDice = false;
            cameraInvoke?.Invoke();
            this.gameObject.GetComponent<Turn>().turn++;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (rollDice)
            {
                Dice.Roll("1d6", "d6-" + diceColor, spawnPoint.transform.position, Force(),diceScale);
                Dice.Roll("1d6", "d6-" + diceColor, spawnPoint.transform.position, Force(),diceScale);
                rollDice = false;
                StartCoroutine(GetDiceScore());
            }
        }
    }
    private IEnumerator GetDiceScore()
    {
        yield return new WaitForSeconds(delay);
        if (!dice._rolling)
        {
            Debug.Log(Dice.AsString(""));
            Die_d6[] isDice = GameObject.FindObjectsOfType<Die_d6>();
            for (int i = 0; i < isDice.Length; i++)
            {
                Debug.Log(isDice[i].value);
                spawnPoint.GetComponent<SpawnPlayer>().diceInt[i] = isDice[i].value;
                hexGrid.turnOnHighLight(isDice[i].value);
                Destroy(isDice[i].gameObject);
            }
            spawnPoint.GetComponent<SpawnPlayer>().spawnTime = true;
        }
    }
    private Vector3 Force()
    {
        Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
        return Vector3.Lerp(spawnPoint.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
    }
}
