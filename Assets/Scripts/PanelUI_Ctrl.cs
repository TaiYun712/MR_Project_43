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

    public Image getPlantBG;
    public Sprite normalPlantBG, pioneerPlantBG;
    public float closeHintTime;

    [Header("植物背包")] 
    public GameObject plantPeckUIPanel;
    public GameObject planPeckShelf;
    public Transform plantPeckUIPos;
    
    [Header("生命力過低提示UI")] 
    public GameObject overDestoryHint;
    public Text overDestoryText;
    public float closeWarningTime;
    
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }
    
    private void Start()
    {
        getPlantPanel.SetActive(false);
        
        plantPeckUIPanel.SetActive(false);
        planPeckShelf.SetActive(false);
        
        overDestoryHint.SetActive(false);

    }

    #region 採集獲取植物面板
    ////顯示採集獲取植物////
    public void ShowGetPlantPanel(Plant plant)
    {
        if(plant == null){return;}
        
        getPlantPanel.SetActive(true);
        PioneerPlantCheck(plant);
        
        getPlantImage.sprite = plant.plantSprite;
        getPlantText.text = plant.plantName;
        
        CancelInvoke();
        Invoke(nameof(CloseHintPanel),closeHintTime);
    }

    public void CloseHintPanel()
    {
        getPlantPanel.SetActive(false);
    }
    
    //檢查是否為先驅植物
    public void PioneerPlantCheck(Plant plant)
    {
        if (plant.isPioneer)
        {
            getPlantBG.sprite = pioneerPlantBG;
        }
        else
        {
            getPlantBG.sprite = normalPlantBG;
        }
    }
    

    #endregion


    #region 植物背包
    public void OpenPlantPeckUI()
    {
        float peckPosHeigh = plantPeckUIPos.transform.position.y;
        plantPeckUIPanel.transform.position = new Vector3(-0.2f, peckPosHeigh-0.3f,1f);
        plantPeckUIPanel.SetActive(true);

        planPeckShelf.transform.position = new Vector3(-0.2f, peckPosHeigh - 0.2f, 0.5f);
        planPeckShelf.SetActive(true);
    }

    public void ClosePlantPeckUI()
    {
        plantPeckUIPanel.SetActive(false);
        planPeckShelf.SetActive(false);
    }

    #endregion

    public void OverDestoryWarningHint(string overReason)
    {
        overDestoryText.text = overReason;
        overDestoryHint.SetActive(true);
        
        Invoke(nameof(CloseWarning),closeWarningTime);
    }

    void CloseWarning()
    {
        overDestoryHint.SetActive(false);
    }
    
    
    
   
    
}
