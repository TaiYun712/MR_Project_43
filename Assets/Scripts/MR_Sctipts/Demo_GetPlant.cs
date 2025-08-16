using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo_GetPlant : MonoBehaviour
{
    public GameObject getPlantPanel;

    public float closeTime = 0.5f;

    void Start()
    {
        getPlantPanel.SetActive(false);
    }

    void Update()
    {
        if (TestBullet.targetIsBroken)
        {
            getPlantPanel.SetActive(true);
            
            
            Invoke("ClosePlantPanel",closeTime);
        }
    }

    public void ClosePlantPanel()
    {
        getPlantPanel.SetActive(false);
    }
}
