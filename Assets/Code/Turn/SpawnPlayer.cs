using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpawnPlayer: MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    public LayerMask selectionMask;
    public UnityEvent<Vector3> PointerClick2;

    [SerializeField] GameObject gameController;
    [SerializeField] GameObject playerPrefab;

    public bool spawnTime = false;
    public int[] diceInt;
    private int playerInt = 0;

    [SerializeField] private HexGrid hexGrid;

    [Header("Delete after this friday")]
    public Sprite[] playerSprite;
    public Sprite[] pIcon;
    public Sprite[] pIconHurt;
    //public GunType[] gunType;

    private void Start()
    {
        gameController = GameObject.Find("GameController");
    }
    void Update()
    {
        DetectMouseClick();
    }
    public void DetectMouseClick()
    {
        if (Input.GetMouseButtonDown(0)&&spawnTime)
        {
            Vector3 mousePos = Input.mousePosition;
            PointerClick2?.Invoke(mousePos);
        }
    }
    public void SpawningPlayer(Vector3 mousePos)
    {
        GameObject result;
        GameObject player;
        if (FindTarget(mousePos, out result))
        {
            Debug.Log(result.transform.position);
            if (result.GetComponent<Hex>().CheckSpawnType())
            {
                if (result.GetComponent<Hex>().CheckHexZone(diceInt[0])||result.GetComponent<Hex>().CheckHexZone(diceInt[1]))
                {
                    Debug.Log("Spawning");
                    playerInt++;
                    Vector3 positionSpawn = new Vector3(result.transform.position.x, 0.2f, result.transform.position.z);
                    player = Instantiate(playerPrefab, positionSpawn, Quaternion.identity);
                    player.GetComponent<Unit>().unit_Num = playerInt;

                    //delete after this friday
                    PlayerStats ps = player.GetComponent<PlayerStats>();
                    ps.playerSprite = playerSprite[playerInt - 1];
                    ps.StartGame();
                    ps.playerIcon = pIcon[playerInt - 1];
                    ps.playerIconHurt = pIconHurt[playerInt - 1];
                    //ps.gunType1 = gunType[playerInt - 1];
                    //end here

                    hexGrid.turnOffHighLight();
                    spawnTime = false;
                    StartCoroutine(DelayRollDice());
                }
            }
        }
    }
    IEnumerator DelayRollDice()
    {
        yield return new WaitForSeconds(1);
        gameController.GetComponent<Start_Turn>().howMuchPlayer--;
        gameController.GetComponent<Start_Turn>().rollDice = true;
    }
    private bool FindTarget(Vector3 mousePos, out GameObject result)
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(mousePos);
        if (Physics.Raycast(ray, out hit, selectionMask))
        {
            result = hit.collider.gameObject;
            Debug.Log("T");
            Debug.Log(result.name);
            return true;
        }
        result = null;
        Debug.Log("F");
        return false;
    }
}
