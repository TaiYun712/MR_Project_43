using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Profiling.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleButtons : MonoBehaviour
{
    [Header("泡泡按鈕")]
    public GameObject settingPanel;
    public Animator settingBoardAni;

    public float quitGameTime = 3f;
    public float startGameTime = 2f;

    public GameObject settingBuble;
    public GameObject startBuble;
    public GameObject quitBuble;
    public GameObject allBubles;

    public GameObject bublePop;
    public Vector3 settingPos;
    public Vector3 startPos;
    public Vector3 quitPos;

    public Transform seePoint;

    public GameObject changeSceneEffect;

    [Header("在地化按鈕")]
    public Text localBtText;
    public Image settingImage;
    public Text languageText;

    public Sprite settingImage_CN,settingImage_EN;
    public string languageText_CN, languageText_EN;
    
    //防誤觸2次
    public float clickLockTime = 0.25f;
    public bool isSwitching = false;
    
    void Start()
    {
        allBubles.SetActive(true);
        Invoke("SetBubleHeight",0.8f);
        settingPanel.SetActive(false);
        settingBoardAni.SetBool("settingIn",false);
        bublePop.SetActive(false);
        changeSceneEffect.SetActive(false);
        
        UpdateBtText();
    }

    #region 泡泡按鈕
      public void SetBubleHeight()   //設置UI高度
    {
        Vector3 eyePos = seePoint.position;
        Vector3 targetPos = new Vector3(eyePos.x, eyePos.y-0.2f, eyePos.z + 0.5f); // Y 是高度, Z 是距離前方
        allBubles.transform.position = targetPos;
        
        settingPos = settingBuble.transform.position;
        startPos = startBuble.transform.position;
        quitPos = quitBuble.transform.position;
    }

      
    //Setting按鈕
    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
        AudioManager.instance.SettingBoardMove();
        settingBoardAni.SetBool("settingIn",true);

        bublePop.transform.position = settingPos;
        bublePop.SetActive(true);
        settingBuble.SetActive(false);

        allBubles.SetActive(false);
        
        UpdateBtText();
        AudioManager.instance.BublePopkeSound();
    }

    public void CloseSettingPanel()
    {
        settingBoardAni.SetBool("settingIn", false);
        AudioManager.instance.SettingBoardMove();
        AudioManager.instance.UISound_Wood();

        bublePop.SetActive(false);
        settingBuble.SetActive(true);

        allBubles.SetActive(true);
        Invoke("HideTheBoard",0.5f);
    }

    public void HideTheBoard()
    {
        settingPanel.SetActive(false);
    }

    
    //Start按鈕
    public void StartGame()
    {
        AudioManager.instance.BublePopkeSound();
        startBuble.SetActive(false);
        
        bublePop.transform.position = startPos;
        bublePop.SetActive(false);
        bublePop.SetActive(true);

        Debug.Log("進入遊戲");
        Invoke("ChangeBubleEffect",2f);
        Invoke("LoadSelectionScene",startGameTime);
    }

    public void ChangeBubleEffect()
    {
        changeSceneEffect.SetActive(true);
        AudioManager.instance.BubleUp();
    }
    
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Power_Destructible Mesh");
        AudioManager.instance.SwitchGameBGM();
    }

    //進入選取場景場景
    public void LoadSelectionScene()
    {
        SceneManager.LoadScene("SceneSelection");
        AudioManager.instance.SwitchSelectionBGM();
    }
    
    //Quit按鈕
    public void QuitGame()
    {
        AudioManager.instance.BublePopkeSound();
        quitBuble.SetActive(false);
        
        bublePop.transform.position = quitPos;
        bublePop.SetActive(false);
        bublePop.SetActive(true);

        Invoke("LeaveTheGame",quitGameTime);
    }

    public void LeaveTheGame()
    {
        Application.Quit();
        Debug.Log("離開遊戲");

    }

    #endregion


    #region 在地化

    public void ChangeLanguage()   // 切換語言
    {
        Debug.Log("A：有進到 ChangeLanguage");

        // 防止同一次操作觸發兩次
        if (isSwitching == true)
        {
            Debug.Log("B：目前 isSwitching = true，所以這次被忽略");
            return;
        }

        // 找不到 LanguageManager 就停止
        if (LanguageManager.instance == null)
        {
            Debug.LogWarning("B：找不到 LanguageManager.instance");
            return;
        }

        // 先鎖住，避免重複點擊
        isSwitching = true;

        Debug.Log("B：目前語言 = " + LanguageManager.instance.currentLanguage);

        // 直接同步切換語言，不用 Coroutine
        if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
        {
            LanguageManager.instance.SetLanguage(LanguageManager.GameLanguage.English);
            Debug.Log("C：語言已切換為英文");
        }
        else
        {
            LanguageManager.instance.SetLanguage(LanguageManager.GameLanguage.Chinese);
            Debug.Log("C：語言已切換為中文");
        }

        UpdateBtText();

        if (AudioManager.instance != null)
        {
            AudioManager.instance.UISound_Wood();
        }

        // 延遲一小段時間後再解除鎖定
        Invoke(nameof(UnlockLanguageSwitch), clickLockTime);
    }

    private void UnlockLanguageSwitch()
    {
        isSwitching = false;
        Debug.Log("D：已解除語言切換鎖定");
    }

    void UpdateBtText()
    {
        Debug.Log("E：進入 UpdateBtText");

        if (localBtText == null)
        {
            Debug.LogWarning("E：localBtText 沒有指定");
            return;
        }

        if (LanguageManager.instance == null)
        {
            Debug.LogWarning("E：找不到 LanguageManager.instance，按鈕先顯示 EN");
            localBtText.text = "EN";
            return;
        }

        if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
        {
            // 目前是中文，所以按鈕顯示 EN，表示按下去會切到英文
            localBtText.text = "EN";
            settingImage.sprite = settingImage_CN;
            languageText.text = languageText_CN;
        }
        else
        {
            // 目前是英文，所以按鈕顯示 TW，表示按下去會切到中文
            localBtText.text = "TW";
            settingImage.sprite = settingImage_EN;
            languageText.text = languageText_EN;
        }

        Debug.Log("E：按鈕文字已更新為 = " + localBtText.text);
    }


    #endregion
   
    
}
