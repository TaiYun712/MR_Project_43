using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantIntroduction : MonoBehaviour
{
    public static PlantIntroduction instance;
    
    public Image introPlantImage;
    public Text introPlantName;
    public Text introPlantPioneer;
    public Text introPlantGrowPower;
    public Text introPlantDescription;

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

    void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenIntroPlantPanel(Plant pickPlant)
    {
        introPlantImage.sprite = pickPlant.plantSprite;
        introPlantName.text = pickPlant.plantName;
        introPlantDescription.text = pickPlant.description;

        introPlantPioneer.text = pickPlant.isPioneer ? "是" : "不是";
        
        
        switch (pickPlant.growPower)
        {
            case 1 :
                introPlantGrowPower.text = "弱";
                break;
            
            case 2 :
                introPlantGrowPower.text = "中";
                break;
            
            case 3 :
                introPlantGrowPower.text = "強";
                break;
        }
        
        gameObject.SetActive(true);
    }

    public void CloseIntroPlantPanel()
    {
        gameObject.SetActive(false);
    }

    
}
