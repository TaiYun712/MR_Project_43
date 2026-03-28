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
    public Text introPlantDescription;

    public GameObject pioneerPlantBg;
    public Image gpImage;
    public Sprite gp_1, gp_2, gp_3;
    
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
        introPlantName.text = pickPlant.GetDisplayName();
        introPlantDescription.text = pickPlant.GetDisplayDescription();

        if (pickPlant.isPioneer)
        {
            pioneerPlantBg.SetActive(true);
        }
        else
        {
            pioneerPlantBg.SetActive(false);
        }
        
        switch (pickPlant.growPower)
        {
            case 1 :
                gpImage.sprite = gp_1;
                break;
            
            case 2 :
                gpImage.sprite = gp_2;
                break;
            
            case 3 :
               gpImage.sprite = gp_3;
                break;
        }
        
        gameObject.SetActive(true);
    }

    public void CloseIntroPlantPanel()
    {
        gameObject.SetActive(false);
    }

    
}
