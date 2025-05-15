using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPosTracking : MonoBehaviour
{
    public GameObject firePf;

    public void FireOn()
    {
        firePf.SetActive(true);
    }

    public void FireOff()
    {
        firePf.SetActive(false);
    }

    void Start()
    {
        firePf.SetActive(false);

    }

    void Update()
    {
        
    }
}
