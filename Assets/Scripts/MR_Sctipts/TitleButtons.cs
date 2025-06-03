using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtons : MonoBehaviour
{
    public GameObject settingPanel;
    public Animator settingBoardAni;

    public float quitGameTime = 3f;

    
    public GameObject settingBuble;
    public GameObject startBuble;
    public GameObject quitBuble;

    public GameObject bublePop;
    public Vector3 settingPos;
    public Vector3 startPos;
    public Vector3 quitPos;

   // public GameObject settingPop;
   // public GameObject startPop;
    //public GameObject quitPop;


    void Start()
    {
        settingPanel.SetActive(false);
        settingBoardAni.SetBool("settingIn",false);

        bublePop.SetActive(false);
        settingPos = settingBuble.transform.position;
        startPos = startBuble.transform.position;
        quitPos = quitBuble.transform.position;

       // settingPop.SetActive(false);
       // startPop.SetActive(false);
        //quitPop.SetActive(false);

    }


    //Setting按鈕
    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
        settingBoardAni.SetBool("settingIn",true);

       // settingPop.SetActive(true);
        bublePop.transform.position = settingPos;
        bublePop.SetActive(true);
        settingBuble.SetActive(false);

        AudioManager.instance.BublePopkeSound();
    }

    public void CloseSettingPanel()
    {
        settingBoardAni.SetBool("settingIn", false);
        // settingPop.SetActive(false);
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
        //startPop.SetActive(true);
        bublePop.transform.position = startPos;
        bublePop.SetActive(true);

        Debug.Log("進入遊戲");
    }

    //Quit按鈕
    public void QuitGame()
    {
        AudioManager.instance.BublePopkeSound();
        quitBuble.SetActive(false);
        // quitPop.SetActive(true);
        bublePop.transform.position = quitPos;
        bublePop.SetActive(true);

        Invoke("LeaveTheGame",quitGameTime);
    }

    public void LeaveTheGame()
    {
        Application.Quit();
        Debug.Log("離開遊戲");

    }

}
