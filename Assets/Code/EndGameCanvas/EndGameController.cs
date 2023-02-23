using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameController : MonoBehaviour
{
    public GameObject a1;
    public GameObject a2;
    public GameObject a3;

    public void EndGameShow()
    {
        a1.SetActive(true);
        a2.SetActive(true);
        a3.SetActive(true);
    }
}
