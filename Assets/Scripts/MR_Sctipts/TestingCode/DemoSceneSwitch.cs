using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoSceneSwitch : MonoBehaviour
{
    public GameObject scenePanel;

    private void Start()
    {
        scenePanel.SetActive(false);
    }

    public void OpenScenePanel()
    {
        bool isActive = scenePanel.activeSelf;
        scenePanel.SetActive(!isActive);
    }
    
    
    
    public void GoToTitle()
    {
        SceneManager.LoadScene("TitleScene");
    }

    public void GOToLandScene()
    {
        SceneManager.LoadScene("LandScene");
    }
    public void GoToSpwanScene()
    {
        SceneManager.LoadScene("SpawnScene");
    }

    public void GoToPoseDemo()
    {
        SceneManager.LoadScene("PowerDEMO");
    }

    public void GoToJanken()
    {
        SceneManager.LoadScene("JankenScene");
    }
    
    
}
