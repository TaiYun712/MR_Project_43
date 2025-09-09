using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public Plant[] allPlants;
    
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void GetPlant()
    {
        if(allPlants.Length == 0){return;}

        Debug.Log("打中採集點，隨機生成植物");
        int index = Random.Range(0, allPlants.Length);
        Plant getPlant = allPlants[index];
        
        PanelUI_Ctrl.instance.ShowGetPlantPanel(getPlant);
        
        PlantInventory.instance.AddPlant(getPlant);
    }
}
