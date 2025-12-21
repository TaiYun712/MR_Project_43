using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habitat_Ctrl : MonoBehaviour
{
    private TileBehaviour tb;
    public bool isLock = false;

    [SerializeField] 
    private GameObject handGrab;

    private void Awake()
    {
        tb = GetComponent<TileBehaviour>();
        
        handGrab.SetActive(true);
    }

    public void OnGrabbed()
    {
        if (isLock)
        {
            handGrab.SetActive(false);
            return;
        }
        
        if(tb == null){return;}
        
        PlacementHelper.instance.OnGrabHabitat(tb);
    }

    public void OnReleased()
    {
        if (isLock) { return;}
        
        PlacementHelper.instance.OnReleaseHabitat();
    }
}
