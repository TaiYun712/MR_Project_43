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

    public Transform lvHintPos;
    public GameObject levelPanel;
    public float showTime;
    public float loadTime;
  
    
    
    void Start()
    {
        Invoke("OpenLevelPanel",loadTime);
        
        Invoke("CloseLevelPanel",showTime);
        
    }

    public void OpenLevelPanel()
    {
        Vector3 lvPanelPos = lvHintPos.transform.position;
        levelPanel.transform.position = lvPanelPos;
        
        levelPanel.SetActive(true);
    }
    
    public void CloseLevelPanel()
    {
        levelPanel.SetActive(false);
        
        SpawnSouls(soulCount); //提示結束後開始生成
    }

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

    public void RespawnSoul()
    {
        Vector3 randomPos = playerPos.position + Random.insideUnitSphere * spawnRadius;
        randomPos.y = Random.Range(0,1.5f);
        GameObject soul = Instantiate(soulPf, randomPos, Quaternion.identity);
        soul.GetComponent<SoulFloating>().spawner = this;

    }

    public void AddSoulCount()
    {
        catchCount++;
        Debug.Log($"收集{catchCount}個碎片");
    }
}
