using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Demo_GetPlant : MonoBehaviour
{
    public GameObject getPlantPanel;
    public Image plantImage;
    public Text plantText;

    public Sprite grass_1, grass_2, grass_3;
    private int plantIndex;
    
    public float closeTime = 0.5f;

    void Start()
    {
        getPlantPanel.SetActive(false);
    }

   

    public void ShowPlantPanel()
    {
        getPlantPanel.SetActive(true);
        RandomPlant();    
            
        Invoke("ClosePlantPanel",closeTime);
    }

    public void ClosePlantPanel()
    {
        getPlantPanel.SetActive(false);
    }

    public void RandomPlant()
    {
        plantIndex = UnityEngine.Random.Range(1, 4);

        switch (plantIndex)
        {
            case 1:
                plantImage.sprite = grass_1;
                plantText.text = "燈心草";
                break;
            
            case 2:
                plantImage.sprite = grass_2;
                plantText.text = "蘆葦";
                break;
            
            case 3:
                plantImage.sprite = grass_3;
                plantText.text = "香蒲";
                break;
            
        }
    }
}
