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
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
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

            if (count > 0 && !spawnPlants.ContainsKey(plant))
            {
                GameObject plantObj = plantPool.GetPlantFromPool(plant);
                plantObj.transform.SetParent(shelfRoot);
                spawnPlants[plant] = plantObj;
                shelfOder.Add(plant);
            }
           
        }
        
        RearrangeShelf();
    }

    
    public void RefreshShelf(Dictionary<Plant, int> inventory)
    {
        RemoveEmptyPlant(inventory);
        
        //更新或新增
        foreach (var kv in inventory)
        {
            Plant plant = kv.Key;
            int count = kv.Value;
            
            if (count > 0 && spawnPlants.ContainsKey(plant))
            {
                GameObject newPlant = plantPool.GetPlantFromPool(plant);
                newPlant.transform.SetParent(shelfRoot);
                spawnPlants[plant] = newPlant;
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
            if (!inventory.ContainsKey(kv.Key) || inventory[kv.Key] <= 0)
            {
                plantPool.ReturnPlantToPool(kv.Key,kv.Value);
                toRemove.Add(kv.Key);
                shelfOder.Remove(kv.Key);
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
            if (spawnPlants.ContainsKey(plant))
            {
                GameObject plantObj = spawnPlants[plant];
                plantObj.transform.localPosition = new Vector3(i * shelfSpacing, 0, 0);
            }
        }
    }

    //收回倒數
    public void StarReturnCountdown(GameObject plantObj, Plant plant, float delay = 2f)
    {
        StartCoroutine(ReturnPlantAfterDelay(plantObj, plant, delay));
        Debug.Log(delay+"開始回收"+plant.plantName);
    }
    
    IEnumerator ReturnPlantAfterDelay(GameObject plantObj,Plant plant,float delay)
    {
        yield return new WaitForSeconds(delay);

        if (plantObj.activeInHierarchy && !plantObj.GetComponent<Plant_Ctrl>().isHeld)
        {
            plantPool.ReturnPlantToPool(plant,plantObj);

            if (!spawnPlants.ContainsKey(plant))
            {
                PlantInventory.instance.AddPlant(plant);
            }
            
        }
    }
   
    
}
