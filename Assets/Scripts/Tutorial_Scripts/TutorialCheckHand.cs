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

    public bool hasPlayCheckSound = false;
    
    void Start()
    {
        countdownTime = holdTime;
        goodHint.fillAmount = 1;
        isGood = false;
        hasPlayCheckSound = false;
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
            if (countdownTime <= 0 && !hasPlayCheckSound)
            {
                AudioManager.instance.SoulCatchOverHint();
                hasPlayCheckSound = true;
            }
            
            StageCheck();
            Debug.Log("完成此教學階段");
        }
    }

    public void HoldingGood()
    {
        isGood = true;
        AudioManager.instance.UISound_Wood();
    }


    public void HoldingNothing()
    {
        isGood = false;
        countdownTime = holdTime;
        goodHint.fillAmount = 1;
    }
    

    void StageCheck()
    {
        if (TutorialManager.instance.currentState == TutorialManager.TutorialState.TutorialOpening)
        {
            TutorialManager.instance.SetState(TutorialManager.TutorialState.LeftSkill);
        }
        
        TutorialManager.instance.CloseCheckHandPanel();
        
    }
}
