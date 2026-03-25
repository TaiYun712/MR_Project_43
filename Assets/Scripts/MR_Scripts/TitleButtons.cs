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
    
    void Start()
    {
        allBubles.SetActive(true);
        Invoke("SetBubleHeight",0.8f);
        
        settingPanel.SetActive(false);
        settingBoardAni.SetBool("settingIn",false);

        bublePop.SetActive(false);
        
        changeSceneEffect.SetActive(false);
        
    }

    //設置UI高度
    public void SetBubleHeight()
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
        Invoke("LoadGameScene",startGameTime);
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
    
}
