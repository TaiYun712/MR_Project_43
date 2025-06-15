using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFloating : MonoBehaviour
{
    public float floatSpeed;
   
    void Start()
    {
        
    }

    void Update()
    {
        transform.position += Vector3.up * floatSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand"))
        {
           AudioManager.instance.CatchTheSoul();
            Destroy(gameObject);

        }
    }
}
