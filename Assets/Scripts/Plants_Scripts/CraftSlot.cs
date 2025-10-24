using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftSlot : MonoBehaviour
{
    public Plant_Ctrl holdCtrl;

    public string plantName;
    public bool isPioneer;
    public int growPower;

    private Collider slotCol;

    private void Awake()
    {
        slotCol = GetComponent<Collider>();
    }
    
    //確認格子占用狀況
    public void Revalidate()
    {
        if (holdCtrl == null ){return;}

        if (!holdCtrl.gameObject.activeInHierarchy)
        {
            ForceClear();
            return;
        }

        if (slotCol != null && !slotCol.bounds.Contains(holdCtrl.transform.position))
        {
            ForceClear();
        }

    }
    
    //切換技能時，把場上所有未使用的植物收回背包
    public void ReturnToPlantPeckAndClear()
    {
        if(holdCtrl == null || holdCtrl.plantData == null){return;}
        
        PlantShelfManager.instance.plantPool.ReturnPlantToPool(holdCtrl.plantData,holdCtrl.gameObject);
        PlantInventory.instance.AddPlant(holdCtrl.plantData);
        
        ForceClear();
    }
    

    //檢查格子中是否有東西
    public bool IsFilled() => holdCtrl != null;
    public Plant GetPlantSO() => holdCtrl ? holdCtrl.plantData : null;

    private void OnTriggerEnter(Collider other)
    {
        TryCapture(other, "Enter");
        holdCtrl.isOnCraft = true;
    }

    private void OnTriggerStay(Collider other)
    {
        // 放手後還在 trigger 裡，可補抓
        TryCapture(other, "Stay");
    }

    private void TryCapture(Collider other, string tag)
    {
        // 抓 ctrl：先從剛體根找，找不到再往父階
        var ctrl = other.attachedRigidbody
            ? other.attachedRigidbody.GetComponent<Plant_Ctrl>()
            : other.GetComponentInParent<Plant_Ctrl>();

        if (ctrl == null || ctrl.plantData == null){ return;}          // 只保護 NRE
        if (holdCtrl != null && holdCtrl != ctrl) {return;}          // 避免覆蓋不同佔用者
        
        
        holdCtrl  = ctrl;
        plantName = ctrl.plantData.plantName;
        isPioneer = ctrl.plantData.isPioneer;
        growPower = ctrl.plantData.growPower;
       

        // （可選）只在第一次成功時印
         Debug.Log($"[Slot {name}] {tag} 放入：{plantName}｜先驅:{isPioneer}｜繁殖力:{growPower}");
    }

    private void OnTriggerExit(Collider other)
    {
        if (holdCtrl == null) return;

        holdCtrl.isOnCraft = false;
        
        var ctrl = other.attachedRigidbody
            ? other.attachedRigidbody.GetComponent<Plant_Ctrl>()
            : other.GetComponentInParent<Plant_Ctrl>();

        if (ctrl != holdCtrl) return;
        
        var leaving = plantName;

        holdCtrl = null;
        plantName = null;
        isPioneer = false;
        growPower = 0;

         Debug.Log($"[Slot {name}] 離開：{leaving}");
    }
    
    //合成後消耗
    public void ClearToPool()
    {
        if(holdCtrl == null || holdCtrl.plantData == null){return;}
        
        PlantShelfManager.instance.plantPool.ReturnPlantToPool(holdCtrl.plantData,holdCtrl.gameObject);

        ForceClear();
    }

    void ForceClear()
    {
        holdCtrl = null;
        plantName = null;
        isPioneer = false;
        growPower = 0;
    }
}