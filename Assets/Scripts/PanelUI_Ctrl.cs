using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelUI_Ctrl : MonoBehaviour
{
    public static PanelUI_Ctrl instance;
    
    [Header("獲取植物UI")]
    public GameObject getPlantPanel;
    public Image getPlantImage;
    public Text getPlantText;
    public float closeHintTime;

    private void Start()
    {
        getPlantPanel.SetActive(false);

    }

    public void ShowGetPlantPanel(Plant plant)
    {
        if(plant == null){return;}
        
        getPlantPanel.SetActive(true);
        getPlantImage.sprite = plant.plantSprite;
        getPlantText.text = plant.plantName;
        
        CancelInvoke();
        Invoke(nameof(CloseHintPanel),closeHintTime);
    }

    public void CloseHintPanel()
    {
        getPlantPanel.SetActive(false);
    }
}
