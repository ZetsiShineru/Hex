using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectItem_Turn : MonoBehaviour
{
    private GameObject gameController;
    private GameObject uiController;
    private GameObject[] players;
    private GameObject player;

    private int healMiNi;
    private int healBig;
    private GunType gType;

    [Header("runSet")]
    private bool prepare = true;
    public bool waitForSelectButton = true;
    [SerializeField]private bool oneTimeSelect = true;

    [Header("Delay")]
    private float delay = 3;
    [SerializeField] private float tDelay = 0;

    private void Start()
    {
        gameController = this.gameObject;
        uiController = GameObject.Find("GamePlayCanvas");
    }
    public void PrepareStartGame()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    public void FindPlayerWithTurn(int p_turn)
    {
        foreach(GameObject p in players)
        {
            if(p.GetComponent<Unit>().unit_Num == p_turn)
            {
                player = p;
            }
        }
    }
    public void StartSelectItem()
    {
        if(prepare && gameController.GetComponent<Turn>().turn == TURNCount.TURN_SelectItem_Phase)
        {
            uiController.GetComponent<UIController>().SetActiveRItemUI(true);
            prepare = false;
        }
        if(tDelay < delay)
        {
            tDelay += Time.deltaTime;
            StartRandomItem();
        }
        else if (tDelay >= delay && oneTimeSelect)
        {
            waitForSelectButton = true;
            uiController.GetComponent<UIController>().SetRandomItem(healMiNi, healBig, gType);
            oneTimeSelect = false;
        }
        
    }
    public void Click(out GunType gunType)
    {
        waitForSelectButton = false;
        gunType = gType;
        PlayerStats p = player.GetComponent<PlayerStats>();
        if((p.item1 + healMiNi) > 4)
        {
            p.item1 = 5;
        }
        else { p.item1 += healMiNi; }
        if ((p.item2 + healBig) > 2)
        {
            p.item2 = 2;
        }
        else { p.item2 += healBig; }
        uiController.GetComponent<UIController>().SetItem(p.item1,p.item2);
        uiController.GetComponent<UIController>().SetActiveRItemUI(false);
    }
    private void StartRandomItem()
    {
        healMiNi = Random.Range(1, 5);
        healBig = Random.Range(0, 2);
        gType = RandomConvertGunType();
        uiController.GetComponent<UIController>().SetRandomItem(healMiNi, healBig, gType);
    }
    private GunType RandomConvertGunType()
    {
        int ranNum = Random.Range(1, 5);
        switch (ranNum)
        {
            case 1:
                return GunType.ShotGun;
                break;
            case 2:
                return GunType.Rifle;
                break;
            case 3:
                return GunType.Smg;
                break;
            case 4:
                return GunType.Sniper;
                break;
        }
        return GunType.Non;
    }
    public void ResetVar(int p_turn)
    {
        prepare = true;
        oneTimeSelect = true;
        tDelay = 0;
        FindPlayerWithTurn(p_turn);
    }
}
