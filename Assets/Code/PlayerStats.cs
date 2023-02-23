using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public static float xOffset = 1, yOffset = 1, zOffset = 0.867f;
    [Header("Gun")]
    public GunType gunType1;
    public GunType gunType2;
    public GameObject shootPoint;

    [Header("Item")]
    public int item1 = 0;
    public int item2 = 0;

    [Header("UI")]
    public Sprite playerSprite;
    public SpriteRenderer playerSpriteRenderer;
    public Sprite playerIcon;
    public Sprite playerIconHurt;
    public int hp = 100;
    public GameObject skinOBJ;

    [Header("Check")]
    public bool selectItem = false;
    bool CheckZone()
    {
        return false;
    }
    public static Vector3Int ConvertPositionToOffset(Vector3 position)
    {
        int x = Mathf.CeilToInt(position.x / xOffset);
        int y = Mathf.RoundToInt(0 / yOffset);
        int z = Mathf.RoundToInt(position.z / zOffset);

        return new Vector3Int(x, y, z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ItemBox")
        {
            selectItem = true;
            Destroy(other.gameObject);
        }
    }
    public void StartGame()
    {
        playerSpriteRenderer.sprite = playerSprite;
    }
    public void ResetVar()
    {
        selectItem = false;
    }
}
