using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleButtons : MonoBehaviour
{
    public GameObject settingPanel;


    void Start()
    {
        settingPanel.SetActive(false);

    }

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
    }
}
