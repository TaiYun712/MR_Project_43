using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFloating : MonoBehaviour
{
    public float floatSpeed;
    public float topHeight;
    public SoulSpawner spawner;

    

    private void Start()
    {
        spawner = FindObjectOfType<SoulSpawner>();
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;

        if (transform.position.y >= topHeight)
        {
            if (spawner != null)
            {
                spawner.RespawnSoul();
            }
            Destroy(gameObject);
        }
    }

    //抓取碎片
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
            if (spawner != null)
            {
                spawner.AddSoulCount();
            }
            
           AudioManager.instance.CatchTheSoul();
            Destroy(gameObject);
        }
    }
}
