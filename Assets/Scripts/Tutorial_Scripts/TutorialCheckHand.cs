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
    private bool hasCheckHand = false;

    [Header("教學關卡完成狀況")] 
    public bool left_IsOver = false;
    public bool skill_1_IsOver = false;
    public bool skill_2_IsOver = false;
    public bool skill_3_IsOver = false;
    public bool tutoria_IsOver = false;
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
        if(hasCheckHand){return;}
        
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

            hasCheckHand = true;
            isGood = false;
            
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
        hasCheckHand = false;
        countdownTime = holdTime;
        goodHint.fillAmount = 1;
        hasPlayCheckSound = false;
    }
    

    void StageCheck()
    {
        if (TutorialManager.instance.currentState == TutorialManager.TutorialState.TutorialOpening && !left_IsOver)
        {
            TutorialManager.instance.SetState(TutorialManager.TutorialState.LeftSkill);
            TutorialManager.instance.Skill_LeftSkill_TutorialDiolog();
            left_IsOver = true;
        }else if (TutorialManager.instance.currentState == TutorialManager.TutorialState.LeftSkill && !skill_1_IsOver)
        {
            TutorialManager.instance.SetState(TutorialManager.TutorialState.Skill_1_Tutorial);
            TutorialManager.instance.Skill_Skill_1_TutorialDiolog();
            skill_1_IsOver = true;
        }else if (TutorialManager.instance.currentState == TutorialManager.TutorialState.Skill_1_Tutorial && !skill_2_IsOver)
        {
            TutorialManager.instance.SetState(TutorialManager.TutorialState.Skill_2_Tutorial);
            TutorialManager.instance.Skill_Skill_2_TutorialDiolog();
            skill_2_IsOver = true;
        }else if (TutorialManager.instance.currentState == TutorialManager.TutorialState.Skill_2_Tutorial && !skill_3_IsOver)
        {
            TutorialManager.instance.SetState(TutorialManager.TutorialState.Skill_3_Tutorial);
            TutorialManager.instance.Skill_Skill_3_TutorialDiolog();
            skill_3_IsOver = true;
        }else if (TutorialManager.instance.currentState == TutorialManager.TutorialState.Skill_3_Tutorial && !tutoria_IsOver)
        {
            TutorialManager.instance.SetState(TutorialManager.TutorialState.TutorialOver);
            TutorialManager.instance.TutorialOver_Diolog();
            tutoria_IsOver = true;
        }
        
        TutorialManager.instance.CloseCheckHandPanel();
        
    }
}
