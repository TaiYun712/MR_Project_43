using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager instance;

    [Header("小精靈對話框")]
    public GameObject diologPanel;
    public Animator diologBeanAni;
    public Text diologText;
    
   // public GameObject mapdiologPanel;

    [Header("確認手勢")] 
    public GameObject checkHandPanel;
    
    [Header("教學影片")] 
    public VideoPlayer videoPlayer;

    public VideoClip mapVideo;
    public VideoClip leftHandVideo;
    
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
      //  mapdiologPanel.SetActive(false);
        checkHandPanel.SetActive(false);

        Skill_Opening_TutorialDiolog();
        videoPlayer.clip = mapVideo;
    }

    //--------  教學階段 對話
    void Skill_Opening_TutorialDiolog()
    {
        Invoke("littleGuyDance",10f);
        ShowTextAfterDelay("泥好啊~人類~ 歡迎來到教學關卡! \n 你將在這裡學習技能的操作方法", 3f);
        ShowTextAfterDelay("為了幫助失去家園的生物們重新回歸 \n 運用技能為牠們打造豐富的棲地吧!", 10f);
        ShowTextAfterDelay("首先來認識遊戲地圖吧!", 17f);
        Invoke("CloseDioLog",20f);
        
    }

    public void Skill_LeftSkill_TutorialDiolog()
    {
        ShowTextAfterDelay("讚喔~接下來來學習切換技能吧~",3f);
        
        Invoke("CloseDioLog",8f);
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
        
        OpenCheckHandPanel(currentState);
    }

    void littleGuyDance()
    {
        diologBeanAni.SetTrigger("isdance");
    }

    /*
    void OpenMapDiolog()
    {
        mapdiologPanel.SetActive(true);
        AudioManager.instance.ShowHint();
        Invoke("CloseMapDiolog",5f);
    }

    void CloseMapDiolog()
    {
        mapdiologPanel.SetActive(false);
        OpenCheckHandPanel(TutorialState.TutorialOpening);
    }
*/
    //---------------------關卡確認手勢
    public void OpenCheckHandPanel(TutorialState currentState)
    {
        if (currentState == TutorialState.TutorialOpening)
        {
            videoPlayer.clip = mapVideo;
        }else if (currentState == TutorialState.LeftSkill)
        {
            videoPlayer.clip = leftHandVideo;
        }
        
        checkHandPanel.SetActive(true);
        AudioManager.instance.ShowHint();
    }

    public void CloseCheckHandPanel()
    {
        checkHandPanel.SetActive(false);
    }

   
}
