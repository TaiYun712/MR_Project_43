using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantRecallManager : MonoBehaviour
{
    public static PlantRecallManager instance;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    //切換技能時，將場上閒置但尚未收回 與 在植物架上尚未合成的植物 直接收回背包
    public void RecallAllUnusePlants()
    {
        //收回合成台上的植物
        if (CraftingManager.instance && CraftingManager.instance.slots != null)
        {
            foreach (var slot in CraftingManager.instance.slots)
            {
                if (slot != null && slot.IsFilled())
                {
                    slot.ReturnToPlantPeckAndClear();
                }
            }
        }

        var snapShot = new List<Plant_Ctrl>(Plant_Ctrl.Active);
        var processed = new HashSet<int>(); //防止重複處理

        foreach (var ctrl in snapShot)
        {
            if(!ctrl || ctrl.plantData == null){continue;}
            if(!ctrl.gameObject.activeInHierarchy){continue;} //合成台上的已回收
            if(ctrl.isOnShelf){continue;} //植物架上的不收

            int id = ctrl.GetInstanceID();
            if(processed.Contains(id)){continue; }
            processed.Add(id);
            
            PlantShelfManager.instance.plantPool.ReturnPlantToPool(ctrl.plantData,ctrl.gameObject);
            PlantInventory.instance.AddPlant(ctrl.plantData);

            ctrl.isHeld = false;
            ctrl.isOnShelf = false;

        }
    }
   
}
