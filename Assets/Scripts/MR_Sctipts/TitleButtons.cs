using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtons : MonoBehaviour
{
    public GameObject settingPanel;
    public float quitGameTime = 3f;

    void Start()
    {
        settingPanel.SetActive(false);

    }


    //Setting«ö¶s
    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
        AudioManager.instance.BublePopkeSound();
    }

    public void CloseSettingPanel()
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
