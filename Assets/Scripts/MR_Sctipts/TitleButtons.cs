using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtons : MonoBehaviour
{
    public GameObject settingPanel;
    public Animator settingBoardAni;

    public float quitGameTime = 3f;


    void Start()
    {
        settingPanel.SetActive(false);
        settingBoardAni.SetBool("settingIn",false);

    }


    //Setting«ö¶s
    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
        settingBoardAni.SetBool("settingIn",true);

        AudioManager.instance.BublePopkeSound();
    }

    public void CloseSettingPanel()
    {
        settingBoardAni.SetBool("settingIn", false);

        Invoke("HideTheBoard",0.5f);
    }

    public void HideTheBoard()
    {
        settingPanel.SetActive(false);
    }

    //Quit«ö¶s
    public void QuitGame()
    {
        AudioManager.instance.BublePopkeSound();
        Invoke("LeaveTheGame",quitGameTime);
    }

    public void LeaveTheGame()
    {
        Application.Quit();
        Debug.Log("Â÷¶}¹CÀ¸");

    }

}
