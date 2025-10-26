using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtCleaning : MonoBehaviour
{
    public GameObject dirtyPf; //用於縮小
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cleaner"))
        {
            //開始清理
            Debug.Log("碰到清潔光束");
        }
    }
}
