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
    
    //進入一般關卡
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
    
    //進入教學關卡
    public void GoToTeachingLeval()
    {
        changeSceneEffect.SetActive(true);
        AudioManager.instance.BubleUp();
        
        Invoke("LoadTeachingLeval",2.0f);
    }

    void LoadTeachingLeval()
    {
        SceneManager.LoadScene("Power_Teaching");
        AudioManager.instance.SwitchTeachingBGM();
    }
}
