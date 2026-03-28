using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant",menuName = "Plants")]
public class Plant : ScriptableObject
{
   public string plantName;
   public string plantName_EN;
   [TextArea]
   public string description;

   [TextArea] public string description_EN;
   
   public Sprite plantSprite;
   
   public bool isPioneer;
   public int growPower;

   public GameObject plantPrefab;
   
   public string GetDisplayName()  //  獲取中/英植物名
   {
      if (LanguageManager.instance == null)
      {
         return plantName;
      }

      if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
      {
         return plantName;
      }
      else
      {
         return plantName_EN;
      }
      
   }

   public string GetDisplayDescription()   //   獲取中/英植物介紹
   {
      if (LanguageManager.instance == null)
      {
         return description;
      }
      
      if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
      {
         return description;
      }
      else
      {
         return description_EN;
      }
   }
}
