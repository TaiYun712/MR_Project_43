using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantSlot : MonoBehaviour
{
   public Plant slotPlant;
   public Image slotPlantImage;
   public Text slotPlantName;
   public Text slotPlantCount;

   public void UpdateCount(int count)
   {
      slotPlantCount.text = count.ToString();
      gameObject.SetActive(count > 0);
   }
}
