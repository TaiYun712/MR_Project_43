using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    public Plant[] allPlants;

    public Transform plantPeckPanel;
    public PlantSlot plantSlotPf;
    
    void Start()
    {
        for (int i = 0; i < allPlants.Length; i++)
        {
            CreateAllPlantSlotOnUIPanel(allPlants[i]);
        }
    }

    void Update()
    {
       
    }

    //擊中採集點時，隨機獲取植物
    public void GetPlant()
    {
        if(allPlants.Length == 0){return;}

        Debug.Log("打中採集點，隨機生成植物");
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
        
        newPlant.gameObject.SetActive(false);
    }
}
