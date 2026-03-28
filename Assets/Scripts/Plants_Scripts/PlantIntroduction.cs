using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlantIntroduction : MonoBehaviour
{
    public static PlantIntroduction instance;
    
    public Image introPlantImage;
    public Text introPlantName;
    public Text introPlantDescription;

    public GameObject pioneerPlantBg;
    public Image pioneerPlantImage;
    public Sprite pioneerPlant_CN, pioneerPlant_EN;
    
    public Image gpImage;
    public Sprite gp_1, gp_2, gp_3;
    public Sprite gpEN_1, gpEN_2, gpEN_3;
    
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
            if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
            {
                pioneerPlantImage.sprite = pioneerPlant_CN;
            }
            else
            {
                pioneerPlantImage.sprite = pioneerPlant_EN;
            }
            
            pioneerPlantBg.SetActive(true);
        }
        else
        {
            pioneerPlantBg.SetActive(false);
        }
        
        switch (pickPlant.growPower)
        {
            case 1 :
                if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
                {
                    gpImage.sprite = gp_1;
                }
                else
                {
                    gpImage.sprite = gpEN_1;
                }
                break;
            
            case 2 :
                if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
                {
                    gpImage.sprite = gp_2;
                }
                else
                {
                    gpImage.sprite = gpEN_2;
                }
                break;
            
            case 3 :
                if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
                {
                    gpImage.sprite = gp_3;
                }
                else
                {
                    gpImage.sprite = gpEN_3;
                }
                break;
        }
        
        gameObject.SetActive(true);
    }

    public void CloseIntroPlantPanel()
    {
        gameObject.SetActive(false);
    }

    
}
