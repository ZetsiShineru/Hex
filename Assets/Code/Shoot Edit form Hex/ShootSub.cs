using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootSub : MonoBehaviour
{
    private Dice dice;

    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private float diceScale;
    public bool rollDice = true;
    private float delay = 5;

    public int[] diceInt;
    public DiceColor diceColor;

    private void Start()
    {
        dice = this.gameObject.GetComponent<Dice>();
    }
    public void RollDice()
    {
            if (rollDice)
            {
                Dice.Roll("1d6", "d6-" + diceColor, spawnPoint.transform.position, Force(), diceScale);
                Dice.Roll("1d6", "d6-" + diceColor, spawnPoint.transform.position, Force(), diceScale);
                rollDice = false;
                StartCoroutine(GetDiceScore());
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
                diceInt[i] = isDice[i].value;
                Destroy(isDice[i].gameObject);
            }
        }
        rollDice = true;
    }
    public bool DoubleCheck()
    {
        if (diceInt[0] == diceInt[1])
        { return true; }
            return false;
    }
    private Vector3 Force()
    {
        Vector3 rollTarget = Vector3.zero + new Vector3(2 + 7 * Random.value, .5F + 4 * Random.value, -2 - 3 * Random.value);
        return Vector3.Lerp(spawnPoint.transform.position, rollTarget, 1).normalized * (-35 - Random.value * 20);
    }
}
