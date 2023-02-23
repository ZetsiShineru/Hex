using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GunType
{
    Non = 0,
    ShotGun = 1,
    Sniper = 6,
    Smg = 2,
    Rifle = 4
}
public class ShootSystem : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;
    public LayerMask selectionMask;

    [SerializeField] private HexGrid hexGrid;
    [SerializeField] private ShootSub sShoot;

    [Header("Gun")]
    private GunType gunType;
    [SerializeField] private GunType gunType1;
    [SerializeField] private GunType gunType2;
    [SerializeField] private GameObject bulletRiflePrefab;
    [SerializeField] private GameObject bulletShotgunPrefab;
    [SerializeField] private GameObject bulletSniperPrefab;
    [SerializeField] private GameObject bulletSmgPrefab;

    private GameObject player;
    [SerializeField] private bool prepareTurn = true;
    [SerializeField] private bool isShooting = false;
    private int player_Turn;
    private BFSResult movementRange = new BFSResult();

    public UIController uiController;

    public void ResetVar()
    {
        prepareTurn = true;
        isShooting = false;
    }
    private void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;
        if (hexGrid == null)
            hexGrid = GameObject.Find("HexGrid").GetComponent<HexGrid>();
        uiController = GameObject.FindObjectOfType<UIController>();
        sShoot = this.gameObject.GetComponent<ShootSub>();
    }
    public void DetectMouseClick(int playerTurn)
    {
        if(prepareTurn)
        {
            player_Turn = playerTurn;
            PrepareShootTurn();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            StartCoroutine(HandleClick(mousePos));
        }
    }
    private void PrepareShootTurn()
    {
        CheckPlayer(out player);
        gunType1 = player.GetComponent<PlayerStats>().gunType1;
        gunType2 = player.GetComponent<PlayerStats>().gunType2;
        DetectButtonClickGun1(); // ref1
        //setUI
        prepareTurn = false;
    }
    public void DetectButtonClickGun1()
    {
        if (prepareTurn == false)
        {
            DisableShootHightlight();
        }
        ShowRange(player.GetComponent<Unit>(), hexGrid,gunType1);
        gunType = gunType1;
    }
    public void DetectButtonClickGun2()
    {
        DisableShootHightlight();
        ShowRange(player.GetComponent<Unit>(), hexGrid,gunType2);
        gunType = gunType2;
    }
    public IEnumerator HandleClick(Vector3 mousePos)
    {
        GameObject result;
        if (FindTarget(mousePos, out result))
        {
            if(CheckRange(result)&&!isShooting)
            {
                isShooting = true;
                sShoot.RollDice();
                //runsome animation
                player.transform.LookAt(result.transform.position);
                GameObject bullet = Instantiate(BulletType(gunType), player.GetComponent<PlayerStats>().shootPoint.transform.position, player.GetComponent<PlayerStats>().shootPoint.transform.rotation);
                DisableShootHightlight();
                yield return new WaitForSeconds(3f);
                Destroy(bullet);
                StartCoroutine(CheckHitEnemy(result));
            }
        }
    }
    public IEnumerator CheckHitEnemy(GameObject result)
    {
        GameObject enemyMat = result.GetComponent<PlayerStats>().skinOBJ;
        Renderer mat = enemyMat.GetComponent<Renderer>();
        mat.material.SetColor("_Color", Color.red);
        yield return new WaitForSeconds(3f);
        mat.material.SetColor("_Color", Color.white);
        result.GetComponent<PlayerStats>().hp -= GunDamage(gunType);
        uiController.SetUIChangePlayer(player_Turn);
        this.gameObject.GetComponent<Turn>().turn++;
    }
    public void DisableShootHightlight()
    {
        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            hexGrid.GetTileAt(hexPosition).DisableHighlight();
        }
    }
    private GameObject BulletType(GunType gunType)
    {
        switch (gunType)
        {
            case GunType.Rifle:
                return bulletRiflePrefab;
                break;
            case GunType.ShotGun:
                return bulletShotgunPrefab;
                break;
            case GunType.Smg:
                return bulletSmgPrefab;
                break;
            case GunType.Sniper:
                return bulletSniperPrefab;
                break;
            default:
                return null;
                break;
        }
        return null;
    }
    private int GunDamage(GunType gunType)
    {
        int damageAdd = 0;
        switch (gunType)
        {
            case GunType.Rifle:
                if (sShoot.DoubleCheck()) { damageAdd = 25; }
                return 25 + damageAdd;
                break;
            case GunType.ShotGun:
                if (sShoot.DoubleCheck()) { damageAdd = 50; }
                return 50 + damageAdd;
                break;
            case GunType.Smg:
                if (sShoot.DoubleCheck()) { damageAdd = 30; }
                return 20 + damageAdd;
                break;
            case GunType.Sniper:
                if (sShoot.DoubleCheck()) { damageAdd = 20; }
                return 30 + damageAdd;
                break;
            default:
                return 0;
                break;
        }
        return 0;
    }
    private void CheckPlayer(out GameObject player)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        foreach (GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            if (p.GetComponent<Unit>().unit_Num == player_Turn)
            {
                player = p;
                return;
            }
        }
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
    private bool CheckRange(GameObject result)
    {
        if(result.gameObject.tag != "Player")
        { return false; }
        hexGrid.GetClosetHex(result.transform.position);
        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            if (hexPosition == hexGrid.GetClosetHex(result.transform.position))
                return true;
        }
        return false;
    }
    private void ShowRange(Unit selectedUnit, HexGrid hexGrid,GunType gType)
    {
        CalcualteRange(selectedUnit, hexGrid,gType);

        Vector3Int unitPos = hexGrid.GetClosetHex(selectedUnit.transform.position);

        foreach (Vector3Int hexPosition in movementRange.GetRangePositions())
        {
            if (unitPos == hexPosition)
                continue;
            hexGrid.GetTileAt(hexPosition).EnableHighlight();
        }
    }
    private void CalcualteRange(Unit selectedUnit, HexGrid hexGrid,GunType gType)
    {
        movementRange = GraphSearch.BFSGetRange(hexGrid, hexGrid.GetClosetHex(selectedUnit.transform.position), ((int)gType), true);
    }
}
