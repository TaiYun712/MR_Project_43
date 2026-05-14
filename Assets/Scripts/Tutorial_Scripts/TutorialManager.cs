using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    public GameObject diologPanel;
    public Text diologText;
    public float firstDiologTime = 3f;
    
    public enum TutorialState
    {
        TutorialOpening,    //開場白
        Skill_1_Tutorial,   //能量球
        Skill_2_Tutorial,   //淨化
        Skill_3_Tutorial,   //棲地合成
        TutorialOver        //結束教程
    }

    public TutorialState currentState = TutorialState.TutorialOpening;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetState(TutorialState newState)
    {
        if (currentState == newState) return;
        
        currentState = newState;
        
        Debug.Log("[GameManager] 現在狀態：" + currentState);
    }
    
    void Start()
    {
        diologPanel.SetActive(false);

        Skill_1_TutorialDiolog();
    }

    void Skill_1_TutorialDiolog()
    {
        ShowTextAfterDelay("泥好啊~人類~ 歡迎來到教學關卡! \n 你將在這裡學習技能的操作方法", firstDiologTime);
        ShowTextAfterDelay("為了幫助失去家園的生物們重新回歸 \n 運用技能為牠們打造豐富的棲地吧!", 10f);
        ShowTextAfterDelay("馬上來學習第一個技能吧! \n 將你的左手掌心面向自己 \n 試著 切換技能 或 開啟資訊面板 吧!", 20f);

        Invoke("SwitchTutorialState_1",15f);
        Invoke("CloseDioLog",28f);
    }

    
    public void ShowTextAfterDelay(string content, float delay)
    {
        StartCoroutine(ShowTextAfterDelayCoroutine(content, delay));
    }
    
    private IEnumerator ShowTextAfterDelayCoroutine(string content, float delay)
    {
        yield return new WaitForSeconds(delay);
        diologPanel.SetActive(true);
        SwitchDiologText(content);
        AudioManager.instance.ShowHint();
    }
    
    void SwitchDiologText(string diolog)
    {
        diologText.text = diolog;
    }

    void CloseDioLog()
    {
        diologPanel.SetActive(false);
    }

    void SwitchTutorialState_1()
    {
        SetState(TutorialState.Skill_1_Tutorial);
        Debug.Log("進入能量球_教學");
    }
}
