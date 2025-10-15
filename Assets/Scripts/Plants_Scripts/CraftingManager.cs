using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager instance;

    [Header("5個合成放置框")] 
    public List<CraftSlot> slots = new List<CraftSlot>(5);
    
    [Header("棲地Prefab")]
    public GameObject wetlandBasicPf;
    public GameObject wetlandAdvancedPf;
    public GameObject habitatBasicPf;
    public GameObject habitatAdvancedPf;
    
    [Header("棲地生成位置")]
    public Transform resultSpawnPos;

    /*
    [Header("合成UI提示")]
    public GameObject craftHint;
    public Text craftHintText;
   */
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else{Destroy(this);}
    }

    public void TryCraft()
    {
        //檢查是否有填滿
        if (!AllFilled())
        {
            Fail("未填滿合成台，不予合成");
            return;
        }

        //取出5格植物資料
        var plantList = new List<Plant>(5);
        foreach (var s in slots)
        {
            var so = s.GetPlantSO();
            if (so == null)
            {
                Fail("有空格或資料異常");
                return;
            }
            
            plantList.Add(so);
        }
        
        //繁殖力限制
        if (!CheckPerSpeciesLimit(plantList, out string reason))
        {
            Fail(reason);
            return;
        }
        
        //判斷 棲地or濕地 & 初階or高階
        bool hasPioneer = false;
        var species = new HashSet<string>(); //用名字判斷種類數
        foreach (var p in plantList)
        {
            if (p.isPioneer) { hasPioneer = true; }

            species.Add(p.plantName);
        }

        bool isWetland = hasPioneer;         //有先趨植物 > 濕地塊
        bool isAdvanced = species.Count > 3; //3種以上植物 > 高階

        //成功合成後收回植物
        foreach (var s in slots)
        {
            s.ClearToPool();
        }
        
        //生成對應棲地
        SpawnResult(isWetland,isAdvanced);
        
        Success($"合成成功：{(isWetland ? "濕地塊" : "棲地塊")} × {(isAdvanced ? "高階" : "初階")}");

    }

    //是否放滿
     bool AllFilled()
     {
         
         if (slots == null || slots.Count != 5)
         {
             return false;
         }

         foreach (var s in slots)
         {
             if (s == null || !s.IsFilled())
             {
                 return false;
             }
         }

         return true;
     }
     
     //是否過量&植物種類數量
     bool CheckPerSpeciesLimit(List<Plant> plants, out string reason)
     {
         //統計每個"種類"出現次數
         var countByName = new Dictionary<string, int>();
         var dataByName = new Dictionary<string, Plant>();

         foreach (var p in plants)
         {
             if (!countByName.ContainsKey(p.plantName))
             {
                 countByName[p.plantName] = 0;
                 dataByName[p.plantName] = p;
             }

             countByName[p.plantName]++;
         }
         
         //檢查是否有植物過量
         foreach (var kv in countByName)
         {
             string name = kv.Key;
             int count = kv.Value;
             int gp = Math.Clamp(dataByName[name].growPower, 1, 3);

             int limit = (gp == 1) ? 3 : (gp == 2) ? 2 : 1;

             if (count > limit)
             {
                 reason = $"{name}過量：growPower={{gp}}，上限 {{limit}}，實際放了 {{count}}";
                 return false;
             }
         }

         reason = null;
         return true;
     }
     
     //生成棲地
     void SpawnResult(bool wetland, bool advanced)
     {
         GameObject pf = null;
         
         if (wetland && !advanced) {pf = wetlandBasicPf;}
         if (wetland &&  advanced) {pf = wetlandAdvancedPf;}
         if (!wetland && !advanced) {pf = habitatBasicPf;}
         if (!wetland &&  advanced) {pf = habitatAdvancedPf;}

         var spawnPos = resultSpawnPos ? resultSpawnPos : transform;
        ////之後改物件池\\\\
         Instantiate(pf, spawnPos.position, spawnPos.rotation); 
     }
    
    //合成失敗提示
    void Fail(string msg)
    {
        Debug.Log("[Craft] " + msg);
    }
    
    
    //合成成功提示
    void Success(string msg)
    {
        Debug.Log("[Craft] " + msg);
    }
}
