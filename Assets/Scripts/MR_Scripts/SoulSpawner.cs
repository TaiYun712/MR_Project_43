using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SoulSpawner : MonoBehaviour
{
    public Transform playerPos;
    public GameObject[] soulPf;
    public int soulCount;
    public float spawnRadius;
    
    public int catchCount = 0;
    public int randomSoulIndex;
    
    public GameObject levelReadyPanel;
    public GameObject levelStartPanel;
    public float showTime;
    public float loadTime;

    public ParticleSystem catchPrt_R,catchPrt_L;

    public GameObject handCatcher_L, handCatcher_R;
    
    void Start()
    { 
        Invoke("OpenLevelPanel",loadTime);
    
       levelStartPanel.SetActive(false);
       levelReadyPanel.SetActive(false);
       
       handCatcher_L.SetActive(true);
       handCatcher_R.SetActive(true);
       
    }
    
 
    //開頭收集碎片提示
    public void OpenLevelPanel()
    {
      levelReadyPanel.SetActive(true);
      AudioManager.instance.ShowHint();
      
      Invoke("CloseLevelPanel",showTime);

      
      GameManager.instance.ToGamePlay();///////////////////////測試用，暫時跳過沒收集完碎片不能用能力的部分
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
            randomSoulIndex = Random.Range(0, soulPf.Length);
            GameObject soul = Instantiate(soulPf[randomSoulIndex], randomPos, Quaternion.identity);
            soul.GetComponent<SoulFloating>().spawner = this;
        }
    }

    //到達一定高度，重新生成碎片
    public void RespawnSoul()
    {
        Vector3 randomPos = playerPos.position + Random.insideUnitSphere * spawnRadius;
        randomPos.y = Random.Range(0,1.5f);
        GameObject soul = Instantiate(soulPf[randomSoulIndex], randomPos, Quaternion.identity);
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
                AudioManager.instance.SoulCatchOverHint();
                Invoke("CloseLvStartPanel",showTime);
            }
        }
        
    }
    
    //抓取碎片效果
    public void PlayCatchSoulParticle()
    {
        catchPrt_R.Play();
        catchPrt_L.Play();
    }
    
    //關閉收集完成提示
    public void CloseLvStartPanel()
    { 
        levelStartPanel.SetActive(false);
        handCatcher_L.SetActive(false);
        handCatcher_R.SetActive(false);
        
        GameManager.instance.ToGamePlay();
    }
}
