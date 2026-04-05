using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelection : MonoBehaviour
{
    public Transform seePoint;
    public GameObject hintsUI;
    public GameObject changeSceneEffect;

    public float holdTime = 3.0f;
    private float countdownTime;

    [Header("一般關卡 手勢UI")] 
    public Image yaHint;
    public bool isYA = false;

    [Header("教學關卡 手勢UI")] 
    public Image goodHint;
    public bool isGood = false;
    
    void Start()
    {
        Invoke("SetHintsHeight",0.5f);
        changeSceneEffect.SetActive(false);
        AudioManager.instance.BubleUp();

        countdownTime = holdTime;
        yaHint.fillAmount = 1;
        goodHint.fillAmount = 1;
    }

    public void SetHintsHeight()
    {
        Vector3 eyePos = seePoint.position;
        Vector3 targetPos = new Vector3(eyePos.x, eyePos.y-0.5f, eyePos.z); // Y 是高度, Z 是距離前方
        hintsUI.transform.position = targetPos;
    }

    private void Update()
    {
       if(!isYA && !isGood){return;}

       if (isYA)
       {
           HoldingGesture(yaHint);
       }
      

       if (isGood)
       {
           HoldingGesture(goodHint);
       }
    }

    void HoldingGesture(Image hintImage)
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            hintImage.fillAmount = countdownTime / holdTime;
        }
        else
        {
            if (isYA)
            {
                GoToNormalLeval();
            }
            else if(isGood)
            {
                GoToTeachingLeval();
            }
        }
    }

    // YA 手勢觸發
    public void HoldingYA()
    {
        if(isGood){return;}
        
        AudioManager.instance.PutPlantInToCraft();
        isYA = true;
        isGood = false;
    }
    
    //  Good 手勢觸發
    public void HoldingGood()
    {
        if(isYA){return;}
        
        AudioManager.instance.PutPlantInToCraft();
        isGood = true;
        isYA = false;
    }
    
    //關閉所有觸發
    public void HoldingNothing()
    {
        isYA = false;
        isGood = false;

        countdownTime = holdTime;
        yaHint.fillAmount = 1;
        goodHint.fillAmount = 1;
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
