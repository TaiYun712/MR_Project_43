using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habitat_Ctrl : MonoBehaviour
{
    private TileBehaviour tb;

    private void Awake()
    {
        
        tb = GetComponent<TileBehaviour>();
        
    }

    public void OnGrabbed()
    {
        PlacementHelper.instance.OnGrabHabitat(tb);
    }

    public void OnReleased()
    {
        PlacementHelper.instance.OnReleaseHabitat();
    }
}
