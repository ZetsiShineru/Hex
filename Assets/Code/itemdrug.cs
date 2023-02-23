using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class itemdrug : MonoBehaviour
{
   public GameObject NumberUiDrugSmall;
   public GameObject NumberUiDrugBig;
   private TextMeshProUGUI NumberUiDrugSmall_text;
   private TextMeshProUGUI NumberUiDrugBig_text;
   public int drugSmall = 0;
   public int drugBig = 0;
   public int maxDrugSmall = 5;
   public int maxDrugBig = 5;

   private void Start()
   {
      NumberUiDrugSmall_text = NumberUiDrugSmall.GetComponent<TextMeshProUGUI>();
      NumberUiDrugBig_text = NumberUiDrugBig.GetComponent<TextMeshProUGUI>();
   }

   private void Update()
   {
      NumberUiDrugSmall_text.text = drugSmall.ToString();
      NumberUiDrugBig_text.text = drugBig.ToString();
   }


   public void drugSmallCollected()
   {
      if (drugSmall < maxDrugSmall)
      {
         drugSmall++;
      }
   }

   public void drugBigCollected()
   {
      if (drugBig < maxDrugBig)
      {
         drugBig++;
      }
   }

   public void useDrugSmall()
   {
      if (drugSmall > 0)
      {
         drugSmall--;
      }
   }

   public void useDrugBig()
   {
      if (drugBig > 0)
      {
         drugBig--;
      }
   }


}
