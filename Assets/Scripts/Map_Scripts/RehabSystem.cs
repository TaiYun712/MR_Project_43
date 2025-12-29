using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RehabSystem : MonoBehaviour
{
    public static RehabSystem instance;

    [Header("基礎生態點&擴張生態點")] 
    public int baseEco = 8;  //棲息條件
    public int expandEco = 5;//擴張條件

    [Header("成功復育階數")] 
    public int targetTotalStages = 3;

    [Header("目前復育階數")] 
    public int totalStages = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    //放下棲地時呼叫
    public void OnMapChange()
    {

        totalStages = 0;

        //檢查富裕階數
        foreach (RegionSystem.RegionInfo r in RegionSystem.instance.EnumerateRegions())
        {
            int stage = CalcStageFromEco(r.ecoSum);
            r.stage = stage;
            totalStages += stage;
        }

        //檢查是否過關
        if (totalStages >= targetTotalStages)
        {
            OnWin();
        }
    }
    
    //計算復育階數
    public int CalcStageFromEco(int ecoSum)
    {
        if (ecoSum < baseEco)
        {
            return 0;
        }

        int extra = ecoSum - baseEco;
        int level = extra / Mathf.Max(1, expandEco);

        if (level < 0) { level = 0; }

        return level;
    }

    //達成復育目標，遊戲通關
    void OnWin()
    {
         Debug.Log("[RehabSystem] 關卡通關！");
         GameManager.instance.ToResult();
         this.enabled = false;
    }
    


    
}
