using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum TURNCount
{
    TURN_Start_Phase = 0,
    TURN_PrepareAfterStart_Phase = 1,
    TURN_Move_Phase = 2,
    TURN_SelectItem_Phase = 3,
    TURN_Battle_Phase = 4,
    TURN_UseItem_Phase = 5,
    Turn_PrePareNextPlayer_Phase = 6
}
public class Turn : MonoBehaviour
{
    [Header("UnityEvent")]
    public UnityEvent<int> findCameraTarget;
    public UnityEvent endGame;
    public GameObject scoreOBJ;

    [Header("Input Script")]
    public PlayerInput pInput_Move;
    public Start_Turn sTurn;
    public ShootSystem shoot_Turn;
    public UIController uiController;
    public SelectItem_Turn selectItem_Turn;

    [Header("Turn")]
    public int player_Turn = 1;
    public int round = 0;
    public TURNCount turn;
    private int howMuchPlayer;
    private GameObject[] players;

    [Header("WTFVARs")]
    public int deadZone = 0;
    public GameObject[] deadZoneBox;

    [Header("CheckZone")]
    public GameObject[] Zone1;
    public GameObject[] Zone2;
    public GameObject[] Zone3;
    public GameObject[] Zone4;
    public GameObject[] Zone5;
    public GameObject[] Zone6;
    private void Start()
    {
        howMuchPlayer = sTurn.howMuchPlayer;
        uiController = GameObject.FindObjectOfType<UIController>();
        players = new GameObject[howMuchPlayer];
    }
    private void Update()
    {
        switch(turn)
        {
            case TURNCount.TURN_Start_Phase:
                sTurn.Prepare_Start();
                break;
            case TURNCount.TURN_PrepareAfterStart_Phase:
                uiController.CollectPlayers();
                uiController.SetUIChangePlayer(player_Turn);
                uiController.EableUI(true);
                players = uiController.players;
                selectItem_Turn.PrepareStartGame();
                selectItem_Turn.FindPlayerWithTurn(player_Turn);
                turn++;
                break;
            case TURNCount.TURN_Move_Phase:
                pInput_Move.DetectMouseClick();
                break;
            case TURNCount.TURN_SelectItem_Phase:
                selectItem_Turn.StartSelectItem();
                break;
            case TURNCount.TURN_Battle_Phase:
                shoot_Turn.DetectMouseClick(player_Turn);
                break;
            case TURNCount.TURN_UseItem_Phase:
                
                break;
            case TURNCount.Turn_PrePareNextPlayer_Phase:
                ResetTurnVar();
                break;
        }
    }
    private void ResetTurnVar()
    {
        if (player_Turn < howMuchPlayer)
        { 
            player_Turn++;
        }
        else { player_Turn = 1; round++; }
        int pDieCount = 0;
        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerStats>().hp < 1)
            {
                pDieCount++;
            }
        }
        bool a = true;
        if (pDieCount == players.Length - 1 && a)
        {
            a = false;
            EndGame();
            return;
        }
        foreach (GameObject p in players)
        {
            if(p.GetComponent<Unit>().unit_Num == player_Turn)
            {
                if(p.GetComponent<PlayerStats>().hp < 1)
                {
                    return;
                }
            }
        }
        SubDeadZone();
        foreach (GameObject p in players)
        {
            if(PlayerZone(p.transform.position) < deadZone+1)
            {
                p.GetComponent<PlayerStats>().hp -= 3;
            }
        }
        findCameraTarget?.Invoke(player_Turn);
        uiController.SetUIChangePlayer(player_Turn);
        selectItem_Turn.ResetVar(player_Turn);
        shoot_Turn.ResetVar();
        foreach (GameObject p in players)
        {
            p.GetComponent<PlayerStats>().ResetVar();
        }
            turn = TURNCount.TURN_Move_Phase;
    }
    int PlayerZone(Vector3 position)
    {
        if(position.x < Zone1[0].transform.position.x && position.x > Zone1[1].transform.position.x && 
           position.z < Zone1[0].transform.position.z && position.z > Zone1[1].transform.position.z)
        { return 1; }
        if (position.x < Zone2[0].transform.position.x && position.x > Zone2[1].transform.position.x &&
           position.z < Zone3[0].transform.position.z && position.z > Zone2[1].transform.position.z)
        { return 2; }
        if (position.x < Zone3[0].transform.position.x && position.x > Zone3[1].transform.position.x &&
           position.z < Zone3[0].transform.position.z && position.z > Zone3[1].transform.position.z)
        { return 3; }
        if (position.x < Zone4[0].transform.position.x && position.x > Zone4[1].transform.position.x &&
           position.z < Zone4[0].transform.position.z && position.z > Zone4[1].transform.position.z)
        { return 4; }
        if (position.x < Zone5[0].transform.position.x && position.x > Zone5[1].transform.position.x &&
           position.z < Zone5[0].transform.position.z && position.z > Zone5[1].transform.position.z)
        { return 5; }
        if (position.x < Zone6[0].transform.position.x && position.x > Zone6[1].transform.position.x &&
           position.z < Zone6[0].transform.position.z && position.z > Zone6[1].transform.position.z)
        { return 6; }
        return 0;
    }
    private int FindLastPlayer()
    {
        foreach (GameObject p in players)
        {
            if (p.GetComponent<PlayerStats>().hp > 0)
            {
                return p.GetComponent<Unit>().unit_Num;
            }
        }
        return 0;
    }
    public void BattleButton()
    {
        SkipTurn(TURNCount.TURN_Battle_Phase);
    }
    public void SkipTurn(TURNCount tc)
    {
        if (((int)tc) > ((int)turn))
        {
            if (turn == TURNCount.TURN_Battle_Phase)
            {
                shoot_Turn.DisableShootHightlight();
            }
            turn = tc;
        }
    }
    public void SubDeadZone()
    {
        if(round > 7) { deadZone = 6; deadZoneBox[5].SetActive(true); }
        else if (round > 6) { deadZone = 5; deadZoneBox[4].SetActive(true); }
        else if (round > 5) { deadZone = 4; deadZoneBox[3].SetActive(true); }
        else if (round > 4) { deadZone = 3; deadZoneBox[2].SetActive(true); }
        else if (round > 3) { deadZone = 2; deadZoneBox[1].SetActive(true); }
        else if (round > 1) { deadZone = 1; deadZoneBox[0].SetActive(true); }

    }
    void EndGame()
    {
        
        Debug.Log($"player{FindLastPlayer()} win");
        scoreOBJ.GetComponent<Score>().winNum = FindLastPlayer();
        uiController.EableUI(false);
        endGame.Invoke();
        //yield return new WaitForSeconds(1);
        //SceneManager.LoadScene("Win");
    }
}
