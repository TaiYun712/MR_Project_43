using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("遊戲階段")]
    public bool isCollecting = false;
    public bool isPlaying = false;
    public bool isEnd = false;
    
    public enum GameState
    {
        SoulCollect, //碎片收集  階段
        GamePlay,    //遊戲  階段
        GameResult   //結算  階段
    }

    public GameState currentState = GameState.SoulCollect;
    
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

    private void Start()
    {
        StartGame();
    }

    public void StartGame(){SetState(GameState.SoulCollect);}
    public void ToGamePlay(){SetState(GameState.GamePlay);}
    public void ToResult(){SetState(GameState.GameResult);}
   
    
    private void SetState(GameState newState)
    {
        if (currentState == newState) return;
        
        currentState = newState;

        // 進入新狀態時要開啟的事情
        OnEnterState(currentState);

        Debug.Log("[GameManager] 現在狀態：" + currentState);
    }

    private void OnEnterState(GameState s)
    {
        switch (s)
        {
            case GameState.SoulCollect:
                CloseALLState();
                isCollecting = true;
                
              
                Debug.Log("收集碎片階段");
                break;

            case GameState.GamePlay:
                CloseALLState();
                isPlaying = true;
                
                Debug.Log("遊戲階段");
                break;

            case GameState.GameResult:
                CloseALLState();
                isEnd = true;
                
                PanelUI_Ctrl.instance.OpenGameOverUI();
                Debug.Log("結算階段");
                break;
        }
    }

    void CloseALLState()
    { 
     isCollecting = false;
     isPlaying = false;
     isEnd = false;
    }

   
}
