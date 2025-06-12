using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtons : MonoBehaviour
{
    public GameObject settingPanel;
    public Animator settingBoardAni;

    public float quitGameTime = 3f;
    public float startGameTime = 2f;

    public GameObject settingBuble;
    public GameObject startBuble;
    public GameObject quitBuble;

    public GameObject bublePop;
    public Vector3 settingPos;
    public Vector3 startPos;
    public Vector3 quitPos;

  

    void Start()
    {
        settingPanel.SetActive(false);
        settingBoardAni.SetBool("settingIn",false);

        bublePop.SetActive(false);
        settingPos = settingBuble.transform.position;
        startPos = startBuble.transform.position;
        quitPos = quitBuble.transform.position;

     
    }


    //Setting按鈕
    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
        settingBoardAni.SetBool("settingIn",true);

          bublePop.transform.position = settingPos;
        bublePop.SetActive(true);
        settingBuble.SetActive(false);

        AudioManager.instance.BublePopkeSound();
    }

    public void CloseSettingPanel()
    {
        settingBoardAni.SetBool("settingIn", false);

        bublePop.SetActive(false);
        settingBuble.SetActive(true);

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
        Invoke("LoadTestScene",startGameTime);
    }

    public void LoadTestScene()
    {
        SceneManager.LoadScene("LandScene");
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
