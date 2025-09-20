using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlantShelfManager : MonoBehaviour
{
    public static PlantShelfManager instance;
    public Transform shelfRoot;
    public PlantPool plantPool;

    private Dictionary<Plant, GameObject> spawnPlants = new Dictionary<Plant, GameObject>();
    
   

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

    public void UpdateShelf(Dictionary<Plant, int> inventory)
    {
        //移除不存在的
        List<Plant> toRemove = new List<Plant>();
               //可縮寫為kv
        foreach (KeyValuePair<Plant,GameObject> kv in spawnPlants)
        {
            if (!inventory.ContainsKey(kv.Key))
            {
               plantPool.ReturnPlantToPool(kv.Key,kv.Value);
               toRemove.Add(kv.Key);
            }
        }

        foreach (var plant in toRemove)
        {
            spawnPlants.Remove(plant);
        }
        
        //更新或新增
        foreach (var kv in inventory)
        {
            Plant plant = kv.Key;
            int count = kv.Value;

            if (!spawnPlants.ContainsKey(plant))
            {
                GameObject plantObj = plantPool.GetPlantFromPool(plant);
                plantObj.transform.SetParent(shelfRoot);
                plantObj.transform.localPosition = Vector3.zero;
                spawnPlants[plant] = plantObj;
                
                Debug.Log("現在架子上有"+count+"個" + plant.plantName);
                
            }
            else
            {
                Debug.Log("現在架子上有"+count+"個" + plant.plantName);
               

            }
        }
    }

   

   
    
}
