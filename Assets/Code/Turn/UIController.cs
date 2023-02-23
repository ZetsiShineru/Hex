using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header ("Stats&Icon")]
    [SerializeField] private Image profileIcon;
    [SerializeField] private Slider hpBar;

    [Header("GunImage For Button")]
    [SerializeField] private Sprite non;
    [SerializeField] private Sprite rifle;
    [SerializeField] private Sprite shotGun;
    [SerializeField] private Sprite smg;
    [SerializeField] private Sprite sniper;

    [Header ("TurnButton")]
    [SerializeField] private Button moveTurn;
    [SerializeField] private Button battleTurn;
    [SerializeField] private Button itemTurn;
    [SerializeField] private Button endTurn;

    [Header("Gun")]
    [SerializeField] private Button gun1;
    [SerializeField] private Button gun2;

    [Header ("Item")]
    [SerializeField] private Button item1;
    [SerializeField] private Text item1Text;
    [SerializeField] private Button item2;
    [SerializeField] private Text item2Text;

    [Header ("RItem")]
    [SerializeField] private GameObject ritem;
    [SerializeField] private GameObject bG;
    [SerializeField] private Image ritem1pic;
    [SerializeField] private Text ritem1tex;
    [SerializeField] private Image ritem2pic;
    [SerializeField] private Text ritem2tex;
    [SerializeField] private Image rGun;
    [SerializeField] private Button closeRItem;


    public GameObject[] players;
    [SerializeField] private GameObject gameController;

    private void Start()
    {
        gameController = GameObject.FindObjectOfType<Turn>().gameObject;
    }
    public void CollectPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    public void SetUIChangePlayer(int playerNum)
    {
        PlayerStats ps;
        foreach (GameObject p in players)
        {
            if(p.GetComponent<Unit>().unit_Num == playerNum)
            {
                ps = p.GetComponent<PlayerStats>();
                if(ps.hp > 50)
                { profileIcon.sprite = ps.playerIcon; }
                else { profileIcon.sprite = ps.playerIconHurt; }
                hpBar.value = ps.hp;
                gun1.gameObject.GetComponent<Image>().sprite = GunImage(ps.gunType1);
                gun2.gameObject.GetComponent<Image>().sprite = GunImage(ps.gunType2);
                item1Text.text = "" + ps.item1;
                item2Text.text = "" + ps.item2;
            }
        }
    }
    public Sprite GunImage(GunType gunType)
    {
        switch(gunType)
        {
            case GunType.Non:
                return non;
                break;
            case GunType.Rifle:
                return rifle;
                break;
            case GunType.ShotGun:
                return shotGun;
                break;
            case GunType.Smg:
                return smg;
                break;
            case GunType.Sniper:
                return sniper;
                break;
            default:
                return non;
                break;
        }
        return non;
    }
    public bool CheckPlayerHP(int playerNum)
    {
        PlayerStats ps;
        foreach (GameObject p in players)
        {
            if (p.GetComponent<Unit>().unit_Num == playerNum)
            {
                ps = p.GetComponent<PlayerStats>();
                if(ps.hp < 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void EableUI(bool setActive)
    {
        profileIcon.gameObject.SetActive(setActive);
        hpBar.gameObject.SetActive(setActive);
        moveTurn.gameObject.SetActive(setActive);
        battleTurn.gameObject.SetActive(setActive);
        itemTurn.gameObject.SetActive(setActive);
        endTurn.gameObject.SetActive(setActive);
        gun1.gameObject.SetActive(setActive);
        gun2.gameObject.SetActive(setActive);
        item1.gameObject.SetActive(setActive);
        item2.gameObject.SetActive(setActive);
    }
    public void SetActiveRItemUI(bool b)
    {
        ritem.SetActive(b);
        bG.SetActive(b);
    }
    public void SetRandomItem(int healMiNi,int healBig,GunType gType)
    {
        rGun.sprite = GunImage(gType);
        ritem1tex.text = "" + healMiNi;
        ritem2tex.text = "" + healBig;
    }
    public void BattleButton()
    {
        gameController.GetComponent<Turn>().SkipTurn(TURNCount.TURN_Battle_Phase);
    }
    public void ButtonGun1()
    {
        if (gameController.GetComponent<Turn>().turn == TURNCount.TURN_Battle_Phase)
        {
            gameController.GetComponent<ShootSystem>().DetectButtonClickGun1();
        }
        else if(gameController.GetComponent<Turn>().turn == TURNCount.TURN_SelectItem_Phase && gameController.GetComponent<SelectItem_Turn>().waitForSelectButton)
        {
            GunType gunType;
            gameController.GetComponent<SelectItem_Turn>().Click(out gunType);
            GameObject player;
            CheckPlayer(out player);
            PlayerStats ps = player.GetComponent<PlayerStats>();
            ps.gunType1 = gunType;
            gun1.gameObject.GetComponent<Image>().sprite = GunImage(gunType);
            gameController.GetComponent<Turn>().turn++;
        }
    }
    public void ButtonGun2()
    {
        if (gameController.GetComponent<Turn>().turn == TURNCount.TURN_Battle_Phase)
        {
            gameController.GetComponent<ShootSystem>().DetectButtonClickGun2();
        }
        else if (gameController.GetComponent<Turn>().turn == TURNCount.TURN_SelectItem_Phase && gameController.GetComponent<SelectItem_Turn>().waitForSelectButton)
        {
            GunType gunType;
            gameController.GetComponent<SelectItem_Turn>().Click(out gunType);
            GameObject player;
            CheckPlayer(out player);
            PlayerStats ps = player.GetComponent<PlayerStats>();
            ps.gunType2 = gunType;
            gun2.gameObject.GetComponent<Image>().sprite = GunImage(gunType);
            gameController.GetComponent<Turn>().turn++;
        }
    }
    public void CloseRItem()
    {
        if (gameController.GetComponent<Turn>().turn == TURNCount.TURN_SelectItem_Phase && gameController.GetComponent<SelectItem_Turn>().waitForSelectButton)
        {
            GunType gunType;
            gameController.GetComponent<SelectItem_Turn>().Click(out gunType);
            gameController.GetComponent<Turn>().turn++;

        }
    }
    public void SetItem(int item1,int item2)
    {
        item1Text.text = "" + item1;
        item2Text.text = "" + item2;
    }
    public void ItemButton1()
    {
        GameObject player;
        CheckPlayer(out player);
        PlayerStats ps = player.GetComponent<PlayerStats>();
        if(ps.item1 > 0 && gameController.GetComponent<Turn>().turn == TURNCount.TURN_UseItem_Phase)
        {
            ps.hp += 10;
            if(ps.hp > 100)
            { ps.hp = 100; }
            ps.item1 -= 1;
            hpBar.value = ps.hp;
            item1Text.text = "" + ps.item1;
            gameController.GetComponent<Turn>().turn++;
        }
    }
    public void ItemButton2()
    {
        GameObject player;
        CheckPlayer(out player);
        PlayerStats ps = player.GetComponent<PlayerStats>();
        if (ps.item2 > 0 && gameController.GetComponent<Turn>().turn == TURNCount.TURN_UseItem_Phase)
        {
            ps.hp += 30;
            if (ps.hp > 100)
            { ps.hp = 100; }
            ps.item2 -= 1;
            hpBar.value = ps.hp;
            item2Text.text = "" + ps.item2;
            gameController.GetComponent<Turn>().turn++;
        }
    }
    private void CheckPlayer(out GameObject player)
    {
        foreach(GameObject p in players)
        {
            if(p.GetComponent<Unit>().unit_Num == gameController.GetComponent<Turn>().player_Turn)
            {
                player = p;
                return;
            }
        }
        player = null;
    }
    public void ItemButton()
    {
        gameController.GetComponent<Turn>().SkipTurn(TURNCount.TURN_UseItem_Phase);
    }
    public void EndButton()
    {
        gameController.GetComponent<Turn>().SkipTurn(TURNCount.Turn_PrePareNextPlayer_Phase);
        gameController.GetComponent<ShootSystem>().DisableShootHightlight();
    }
}
