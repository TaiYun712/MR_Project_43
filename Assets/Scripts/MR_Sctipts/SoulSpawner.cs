using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpawner : MonoBehaviour
{
    public Transform playerPos;
    public GameObject soulPf;
    public int soulCount;
    public float spawnRadius;
    
    public int catchCount = 0;

   // public Transform hintPos;
    public GameObject levelReadyPanel;
    public GameObject levelStartPanel;
    public float showTime;
    public float loadTime;

    public GameObject soulBt; //demo用，去收集碎片按鈕
    
    void Start()
    {
       // Invoke("OpenLevelPanel",loadTime);
       soulBt.SetActive(false); 
       
        levelStartPanel.SetActive(false);
        
    }
    
    //觸發收集碎片按鈕
    public void GoToSoulPart()
    {
       
        bool isActive = soulBt.activeSelf;
        soulBt.SetActive(!isActive);
    }

    //開頭收集碎片提示
    public void OpenLevelPanel()
    {
      //  Vector3 lvPanelPos = lvHintPos.transform.position;
      //  levelPanel.transform.position = lvPanelPos;
      soulBt.SetActive(false); //demo用
      
      levelReadyPanel.SetActive(true);
      AudioManager.instance.ShowHint();
      
      Invoke("CloseLevelPanel",showTime);

    }
    
    //關閉提示
    public void CloseLevelPanel()
    {
        levelReadyPanel.SetActive(false);
        
        SpawnSouls(soulCount); //提示結束後開始生成
    }

    //生成碎片
    public void SpawnSouls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPos = playerPos.position + Random.insideUnitSphere * spawnRadius;
            randomPos.y = Random.Range(0,1.5f);
            GameObject soul = Instantiate(soulPf, randomPos, Quaternion.identity);
            soul.GetComponent<SoulFloating>().spawner = this;
        }
    }

    //到達一定高度，重新生成碎片
    public void RespawnSoul()
    {
        Vector3 randomPos = playerPos.position + Random.insideUnitSphere * spawnRadius;
        randomPos.y = Random.Range(0,1.5f);
        GameObject soul = Instantiate(soulPf, randomPos, Quaternion.identity);
        soul.GetComponent<SoulFloating>().spawner = this;

    }

    //計算碎片收集數量
    public void AddSoulCount()
    {
        if (catchCount < soulCount)
        {
            catchCount++;
            Debug.Log($"收集{catchCount}個碎片");

            if (catchCount >= soulCount)
            {
                Debug.Log("收集完畢");
                levelStartPanel.SetActive(true);
                AudioManager.instance.PlayRedbirdSound();
                Invoke("CloseLvStartPanel",showTime);
            }
        }
        
    }
    
    //關閉收集完成提示
    public void CloseLvStartPanel()
    {
        levelStartPanel.SetActive(false);
    }
}
