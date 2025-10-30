using System;
using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;


public class PlantShelfManager : MonoBehaviour
{
    public static PlantShelfManager instance;
    public Transform shelfRoot;
    public PlantPool plantPool;
    
    private Dictionary<Plant, GameObject> spawnPlants = new Dictionary<Plant, GameObject>();
    
    private List<Plant> shelfOder = new List<Plant>(); //控制排列
    public float shelfSpacing = 0.1f;

    public float returnDelay = 2f;
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    //當架上的植物被拿起
    public void PickPlantFromShelf(Plant plant,GameObject pickObj)
    {
        if (spawnPlants.TryGetValue(plant, out var current) && current == pickObj)
        {
            spawnPlants[plant] = null;
        }
    }

    //更新架子/移除或新增
    public void UpdateShelf(Dictionary<Plant, int> inventory)
    {
        RemoveEmptyPlant(inventory);
        
        //更新或新增
        foreach (var kv in inventory)
        {
            Plant plant = kv.Key;
            int count = kv.Value;
            
            if(count <= 0){continue;}

            //若為首次加入，新增欄位
            if(!shelfOder.Contains(plant)){shelfOder.Add(plant);}

            //持有數尚未歸零
            if (!spawnPlants.ContainsKey(plant) || spawnPlants[plant] == null)
            {
                GameObject plantObj = plantPool.GetPlantFromPool(plant);
                plantObj.transform.SetParent(shelfRoot,false);
                plantObj.transform.localPosition = Vector3.zero;
                plantObj.transform.localRotation = Quaternion.identity;
                plantObj.transform.localScale = Vector3.one;

                var ctrl = plantObj.GetComponent<Plant_Ctrl>();
                if (ctrl != null)
                {
                    ctrl.isOnShelf = true;
                    ctrl.isHeld = false;
                    ctrl.isOnCraft = false;
                    ctrl.basePlate?.SetActive(true);
                    ctrl.gpHint?.SetActive(true);
                }

                spawnPlants[plant] = plantObj;

                int idx = shelfOder.IndexOf(plant);
                plantObj.transform.localPosition = new Vector3(idx * shelfSpacing, 0f, 0f);
            }
        }
        
        RearrangeShelf();
    }
    

    //移除不存在的植物
    public void RemoveEmptyPlant(Dictionary<Plant, int> inventory)
    {
        List<Plant> toRemove = new List<Plant>();
        //可縮寫為kv
        foreach (KeyValuePair<Plant,GameObject> kv in spawnPlants)
        {
            Plant plant = kv.Key;
            GameObject obj = kv.Value;

            bool isNoMore = !inventory.ContainsKey(kv.Key) || inventory[plant] <= 0;

            if (isNoMore)
            {
                if (obj != null)
                {
                    var ctrl = obj.GetComponent<Plant_Ctrl>();
                    if (ctrl == null || !ctrl.isHeld)
                    {
                        plantPool.ReturnPlantToPool(plant,obj);
                    }
                }
                
                toRemove.Add(plant);
                shelfOder.Remove(plant);
            }
        }

        foreach (var plant in toRemove)
        {
            spawnPlants.Remove(plant);
        }

    }

   
    //重新排列架子
    void RearrangeShelf()
    {
        for (int i = 0; i < shelfOder.Count; i++)
        {
            Plant plant = shelfOder[i];
            if (spawnPlants.TryGetValue(plant,out var obj) && obj != null)
            {
                obj.transform.localPosition = new Vector3(i * shelfSpacing, 0f, 0f);
                obj.transform.localRotation = Quaternion.identity;
                obj.transform.localScale = Vector3.one;
            }
        }
    }

    //收回倒數
    public void StarReturnCountdown(GameObject plantObj, Plant plant, float delay = 2f)
    {
        StartCoroutine(ReturnPlantAfterDelay(plantObj, plant, delay));
       // Debug.Log(delay+"開始回收"+plant.plantName);
    }
    
    IEnumerator ReturnPlantAfterDelay(GameObject plantObj,Plant plant,float delay)
    {
        yield return new WaitForSeconds(delay);

        if (plantObj.activeInHierarchy && !plantObj.GetComponent<Plant_Ctrl>().isHeld)
        {
            plantPool.ReturnPlantToPool(plant,plantObj);
            PlantInventory.instance.AddPlant(plant);
            
        }
    }

  

   
   
    
}
