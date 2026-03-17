using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PanelUI_Ctrl : MonoBehaviour
{
    public static PanelUI_Ctrl instance;
    
    [Header("獲取植物UI")]
    public GameObject getPlantPanel;
    public Image getPlantImage;
    public Text getPlantText;

    public Image getPlantBG;
    public GameObject getPioneerHint;
    public float closeHintTime;

    [Header("植物背包")]
    public GameObject landSkillUI; 
    public Transform plantPeckUIPos;
    
    [Header("生命力過低提示UI")] 
    public GameObject overDestoryHint;
    public Text overDestoryText;
    public float closeWarningTime;

    [Header("遊戲結束提示")] 
    public GameObject gameOverPanel;
    public float closeGameOverUITime;

    [Header("對話框指引")] 
    public GameObject diologPanel;
    public Text diologText;
    public float dioShowTime;
    
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }
    
    private void Start()
    {
        getPlantPanel.SetActive(false);
        getPioneerHint.SetActive(false);
        
        landSkillUI.SetActive(false);
        
        overDestoryHint.SetActive(false);
        
        gameOverPanel.SetActive(false);
        
        diologPanel.SetActive(false);

    }

    #region 採集獲取植物面板
    ////顯示採集獲取植物////
    public void ShowGetPlantPanel(Plant plant)
    {
        if(plant == null){return;}
        
        PioneerPlantCheck(plant);
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
    
    //檢查是否為先驅植物
    public void PioneerPlantCheck(Plant plant)
    {
        if (plant.isPioneer)
        {
           getPioneerHint.SetActive(true);
        }
        else
        {
            getPioneerHint.SetActive(false);
        }
    }
    

    #endregion


    #region 植物背包
    public void OpenPlantPeckUI()
    {
        if(GameManager.instance.currentState == GameManager.GameState.GameResult){return;}
        
        float peckPosHeigh = plantPeckUIPos.transform.position.y;

        //拿掉玩家抬頭或低頭角度，只取正前方水平位置
        Vector3 forwardDir = plantPeckUIPos.forward;
        forwardDir.y = 0f;
        forwardDir.Normalize();

        Vector3 uiPos = new Vector3(plantPeckUIPos.position.x, peckPosHeigh - 0.3f, plantPeckUIPos.position.z);
        uiPos = uiPos + forwardDir * 0.3f;
        landSkillUI.transform.position = uiPos;

        Vector3 lookTarget = new Vector3(plantPeckUIPos.position.x, landSkillUI.transform.position.y,
            plantPeckUIPos.position.z);
        
        landSkillUI.transform.LookAt(lookTarget);
        landSkillUI.transform.Rotate(0f,180f,0f); //因為UI正面是反的所以要再翻面
        
        landSkillUI.SetActive(true);
   
    }

    public void ClosePlantPeckUI()
    {
        landSkillUI.SetActive(false);
        
    }

    #endregion

    public void OverDestoryWarningHint(string overReason)
    {
        overDestoryText.text = overReason;
        overDestoryHint.SetActive(true);
        
        Invoke(nameof(CloseWarning),closeWarningTime);
    }

    public void CloseWarning()
    {
        overDestoryHint.SetActive(false);
    }

    public void OpenGameOverUI()
    {
        ClosePlantPeckUI();
        
        gameOverPanel.SetActive(true);
        AudioManager.instance.ShowHint();
        AudioManager.instance.PlayWinBGM();
        
        Invoke("CloseGameOverUI",closeGameOverUITime);
    }

    void CloseGameOverUI()
    {
        gameOverPanel.SetActive(false);
        SceneManager.LoadScene("TitleScene");
        AudioManager.instance.SwitchTitleBGM();
    }
    
    //------對話框指引
    public void ShowDiolog(string dioContent)
    {
        diologText.text = dioContent;
        diologPanel.SetActive(true);
        AudioManager.instance.ShowHint();
        
        Invoke("CloseDiolog",dioShowTime);
    }

    void CloseDiolog()
    {
        diologPanel.SetActive(false);
    }
   
    
}
