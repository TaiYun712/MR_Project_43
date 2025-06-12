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
}
