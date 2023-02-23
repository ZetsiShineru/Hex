using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameObject PotionUI;

    private itemdrug _itemdrug;

    public bool isSmallPotion = false;
    // Start is called before the first frame update
    void Start()
    {
        _itemdrug = PotionUI.GetComponent<itemdrug>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isSmallPotion)
        {
            _itemdrug.drugSmallCollected();
        }
        else
        {
            _itemdrug.drugBigCollected();
        }
        Destroy(this.gameObject);
    }
    
}
