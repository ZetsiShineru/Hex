using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int winNum = 1;
    public Sprite[] player;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public Sprite SelectSprite()
    {
        return player[winNum-1];
    }
}
