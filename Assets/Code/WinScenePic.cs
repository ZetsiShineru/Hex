using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScenePic : MonoBehaviour
{
    public Image player;
    public Score scoreEnemy;
    private void Start()
    {
        
    }
    private void Update()
    {
        //scoreEnemy = GameObject.FindObjectOfType<Score>();
        player.sprite = scoreEnemy.SelectSprite();
    }
}
