using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantPool : MonoBehaviour
{
    private Dictionary<Plant, Queue<GameObject>> poolDict = new Dictionary<Plant, Queue<GameObject>>();
    
    
    //初始化某植物池
    public void InitPlantPool(Plant plant, int size = 20)
    {
        if(poolDict.ContainsKey(plant))return;

        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject plantObj = Instantiate(plant.plantPrefab, transform);
            plantObj.SetActive(false);
            queue.Enqueue(plantObj);
        }

        poolDict[plant] = queue;
    }

    //從池中拿出植物
    public GameObject GetPlantFromPool(Plant plant)
    {
        if(!poolDict.ContainsKey(plant)){InitPlantPool(plant);}

        var queue = poolDict[plant];
        if (queue.Count > 0)
        {
            GameObject plantObj = queue.Dequeue();
            plantObj.SetActive(true);
            return plantObj;
        }
        else
        {
            return Instantiate(plant.plantPrefab);
        }
    }
    
    //將植物放回植物池
    public void ReturnPlantToPool(Plant plant,GameObject plantObj)
    {
        plantObj.SetActive(false);
        plantObj.transform.SetParent(transform);
        poolDict[plant].Enqueue(plantObj);
    }
}
