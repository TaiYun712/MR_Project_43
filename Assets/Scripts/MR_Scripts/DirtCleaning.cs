using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DirtCleaning : MonoBehaviour
{
    public DestructibleEnvironment_Manager destructibleManager;
    public Transform dirtyPf; //用於縮小
    Vector3 initialLocalScale;

    [Header("清潔設定")] 
    public float maxCleanTime = 3;
    public float minCleanTime = 1;
    public float cleanTime; //本次清理時間

    public float clearSize = 0.02f;
    
    private bool isCleaning;    //是否正在清理
    private float process = 0f; //清潔累積
    private bool hasPlayHitSound = false;

    private void Awake()
    {
        destructibleManager = FindObjectOfType<DestructibleEnvironment_Manager>();
    }

    private void OnEnable()
    {
        process = 0f;
        isCleaning = false;

        if (dirtyPf != null)
        {
            initialLocalScale = dirtyPf.localScale;
        }
        
        cleanTime = Random.Range(minCleanTime,maxCleanTime);
        if (cleanTime <= 0f) {cleanTime = 1f;}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cleaner"))
        {
            isCleaning = true;
            
            AudioManager.instance.CleanHit();
            hasPlayHitSound = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Cleaner"))
        {
            isCleaning = false;
            
            hasPlayHitSound = false;
        }    
    }

    private void Update()
    {
        if(!isCleaning){return;}

        process = process + Time.deltaTime;
        if (process < 0f) { process = 0; }

        float t;
        if (cleanTime > 0) { t = process / cleanTime; }
        else { t = 1; }

        if (t > 1) { t = 1; }

        if (dirtyPf != null)
        {
            dirtyPf.localScale = Vector3.Lerp(initialLocalScale, Vector3.zero, t);
            Vector3 s = dirtyPf.localScale;
            if (Mathf.Abs(s.x) <= clearSize)
            {
                DirtyClear();
                return;
            }
        }
        else  if (t >= 1 )
        {
            DirtyClear();
        }
        
    }

    void DirtyClear()
    {
        destructibleManager.currentDirtyCount--;
        Destroy(this.gameObject);
    }
}
