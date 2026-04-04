using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelection : MonoBehaviour
{
    public Transform seePoint;
    public GameObject hintsUI;
    
    public GameObject changeSceneEffect;
    
    void Start()
    {
        Invoke("SetHintsHeight",0.5f);
        
        changeSceneEffect.SetActive(false);
        AudioManager.instance.BubleUp();
    }

    public void SetHintsHeight()
    {
        Vector3 eyePos = seePoint.position;
        Vector3 targetPos = new Vector3(eyePos.x, eyePos.y-0.5f, eyePos.z); // Y 是高度, Z 是距離前方
        hintsUI.transform.position = targetPos;
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
