using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Plant",menuName = "Plants")]
public class Plant : ScriptableObject
{
   public string plantName;
   public string description;
   public Sprite plantSprite;
   
   public bool isPioneer;
   public int growPower;

   public GameObject plantPrefab;
}
