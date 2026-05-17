using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [Header("小精靈對話框")]
    public GameObject diologPanel;
    public Animator diologBeanAni;
    public Text diologText;
    public float firstDiologTime = 3f;
    
    public GameObject mapdiologPanel;

    [Header("確認手勢")] 
    public GameObject checkHandPanel;
    
    public enum TutorialState
    {
        TutorialOpening,    //開場白&地圖
        LeftSkill,          //左手用法
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
        mapdiologPanel.SetActive(false);
        checkHandPanel.SetActive(false);

        Skill_1_TutorialDiolog();
    }

    void Skill_1_TutorialDiolog()
    {
        Invoke("littleGuyDance",8f);
        ShowTextAfterDelay("泥好啊~人類~ 歡迎來到教學關卡! \n 你將在這裡學習技能的操作方法", firstDiologTime);
        ShowTextAfterDelay("為了幫助失去家園的生物們重新回歸 \n 運用技能為牠們打造豐富的棲地吧!", 8f);
        Invoke("CloseDioLog",15f);
        
        Invoke("OpenMapDiolog",15f);
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

    void littleGuyDance()
    {
        diologBeanAni.SetTrigger("isdance");
    }

    void OpenMapDiolog()
    {
        mapdiologPanel.SetActive(true);
        AudioManager.instance.ShowHint();
        Invoke("CloseMapDiolog",5f);
    }

    void CloseMapDiolog()
    {
        mapdiologPanel.SetActive(false);
        OpenCheckHandPanel();
    }

    //---------------------關卡確認手勢
    public void OpenCheckHandPanel()
    {
        checkHandPanel.SetActive(true);
        AudioManager.instance.ShowHint();
    }

    public void CloseCheckHandPanel()
    {
        checkHandPanel.SetActive(false);
    }

   
}
