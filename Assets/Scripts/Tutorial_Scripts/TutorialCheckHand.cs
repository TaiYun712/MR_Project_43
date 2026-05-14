using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialCheckHand : MonoBehaviour
{
    [Header("手勢UI")] 
    public Image goodHint;
    public bool isGood = false;
    public float holdTime = 3.0f;
    private float countdownTime;
    
    void Start()
    {
        countdownTime = holdTime;
        goodHint.fillAmount = 1;
        isGood = false;
    }

    void Update()
    {
        if (isGood)
        {
            HoldingGesture();
        }
    }

    void HoldingGesture()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            goodHint.fillAmount = countdownTime / holdTime;
        }
        else
        {
            Debug.Log("完成此教學階段");
        }
    }

    public void HoldingGood()
    {
        isGood = true;
    }


    public void HoldingNothing()
    {
        isGood = false;
        countdownTime = holdTime;
        goodHint.fillAmount = 1;
    }
}
