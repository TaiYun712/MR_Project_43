using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlantManager : MonoBehaviour
{
    public static PlantManager instance;
    
    public Plant[] allPlants;

    public Transform plantPeckPanel;
    public PlantSlot plantSlotPf;

    private Dictionary<Plant, PlantSlot> plantSlots = new Dictionary<Plant, PlantSlot>();

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    void Start()
    {
        for (int i = 0; i < allPlants.Length; i++)
        {
            CreateAllPlantSlotOnUIPanel(allPlants[i]);
        }
    }
    

    //擊中採集點時，隨機獲取植物
    public void GetPlant()
    {
        if(allPlants.Length == 0){return;}

        //Debug.Log("打中採集點，隨機生成植物");
        int index = Random.Range(0, allPlants.Length);
        Plant getPlant = allPlants[index];
        
        PanelUI_Ctrl.instance.ShowGetPlantPanel(getPlant);
        
        PlantInventory.instance.AddPlant(getPlant);
    }

    //在開始就生成所有植物的UI並隱藏
    public void CreateAllPlantSlotOnUIPanel(Plant plant)
    {
        PlantSlot newPlant = Instantiate(plantSlotPf,plantPeckPanel,false);

        newPlant.slotPlant = plant;
        newPlant.slotPlantImage.sprite= plant.plantSprite;
        newPlant.slotPlantName.text = plant.plantName;
        newPlant.slotPlantCount.text = "0";
        newPlant.UpdateCount(0);

        plantSlots[plant] = newPlant;
    }

    public void UpdatePlantPeckUI(Plant plant,int count)
    {
        if (plantSlots.ContainsKey(plant))
        {
            plantSlots[plant].UpdateCount(count);
        }
    }
}
