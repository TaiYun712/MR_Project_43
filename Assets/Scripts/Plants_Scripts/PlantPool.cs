using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction;
using UnityEngine;

public class PlantPool : MonoBehaviour
{
    private Dictionary<Plant, Queue<GameObject>> poolDict = new Dictionary<Plant, Queue<GameObject>>();
    
    
    //初始化某植物池
    public void InitPlantPool(Plant plant, int size = 10)
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
        GameObject plantObj = queue.Count > 0 ? queue.Dequeue() : Instantiate(plant.plantPrefab);
        plantObj.SetActive(true);
        
        return plantObj;
       
    }
    
    //將植物放回植物池
    public void ReturnPlantToPool(Plant plant,GameObject plantObj)
    {
        plantObj.SetActive(false);
        plantObj.transform.SetParent(transform);
        poolDict[plant].Enqueue(plantObj);
    }
}
