using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant_Ctrl : MonoBehaviour
{
    public static readonly HashSet<Plant_Ctrl> Active = new HashSet<Plant_Ctrl>();
    
    public Plant plantData;
    public GameObject basePlate;

    public bool isHeld;
    public bool isOnShelf;

    private void OnEnable() { Active.Add(this);}
    private void OnDisable() { Active.Remove(this);}
   


    void Start()
    {
       // Debug.Log("這是個" + plantData.plantName);
        basePlate.SetActive(true);
        isHeld = false;
        
    }

    public void PickUpThePlant()
    {
       // Debug.Log("拿起" + plantData.plantName);

        if (isOnShelf)
        {
            PlantShelfManager.instance.PickPlantFromShelf(plantData,this.gameObject);
            isOnShelf = false;
            
            PlantInventory.instance.RemovePlant(plantData);
        }
        
        PlantIntroduction.instance.OpenIntroPlantPanel(plantData);//顯示植物說明
        basePlate.SetActive(false);
        isHeld = true;
        
    }

    public void PutDownThePlant()
    {
       PlantIntroduction.instance.CloseIntroPlantPanel();//關閉植物說明
       isHeld = false;
       
       PlantShelfManager.instance.StarReturnCountdown(this.gameObject,plantData);
    }
    
    
}
