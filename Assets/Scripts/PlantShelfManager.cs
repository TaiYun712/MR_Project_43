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
        //移除不存在的
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
        
        //更新或新增
        foreach (var kv in inventory)
        {
            Plant plant = kv.Key;
            int count = kv.Value;

            if (count > 0 && !spawnPlants.ContainsKey(plant))
            {
                GameObject plantObj = plantPool.GetPlantFromPool(plant);
                plantObj.transform.SetParent(shelfRoot);
                //plantObj.transform.localPosition = Vector3.zero;
                spawnPlants[plant] = plantObj;
                
                shelfOder.Add(plant);
                
                Debug.Log("現在架子上有"+count+"個" + plant.plantName);
                
            }
            else
            {
                Debug.Log("現在架子上有"+count+"個" + plant.plantName);
            }
        }


        RearrangeShelf();
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

   
    
}
