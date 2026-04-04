using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public GameObject changeSceneEffect;
    
    void Start()
    {
        changeSceneEffect.SetActive(false);
        AudioManager.instance.BubleUp();
    }

    void Update()
    {
        
    }

    public void GoToNormalLeval()
    {
        changeSceneEffect.SetActive(true);
        AudioManager.instance.BubleUp();
        Invoke("LoadNormalLeval",2.0f);
    }

    void LoadNormalLeval()
    {
        SceneManager.LoadScene("Power_Destructible Mesh");
        AudioManager.instance.SwitchGameBGM();
    }
}
