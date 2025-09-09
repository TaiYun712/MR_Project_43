using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantInventory : MonoBehaviour
{
    public static PlantInventory instance;
    private Dictionary<Plant, int> inventory = new Dictionary<Plant, int>();
    
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

    //增加植物數量
    public void AddPlant(Plant plant, int amount = 1)
    {
        if (inventory.ContainsKey(plant))
        {
            inventory[plant] += amount;
        }
        else
        {
            inventory[plant] = amount;
        }
        
        PlantShelfManager.instance.UpdateShelf(inventory);
    }


    //減少植物數量
    public void RemovePlant(Plant plant, int amount = 1)
    {
        if(!inventory.ContainsKey(plant)){return;}

        inventory[plant] -= amount;
        if (inventory[plant] <= 0)
        {
            inventory.Remove(plant);
        }
        
        PlantShelfManager.instance.UpdateShelf(inventory);
    }
    
    //獲得植物數量
    public int GetCount(Plant plant)
    {
        return inventory.ContainsKey(plant) ? inventory[plant] : 0;
    }
   
}
