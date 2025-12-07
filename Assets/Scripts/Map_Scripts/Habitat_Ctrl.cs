using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Habitat_Ctrl : MonoBehaviour
{
    public PlacementHelper placer;
    private TileBehaviour tb;

    private void Awake()
    {
        tb = GetComponent<TileBehaviour>();
        if (placer == null)
        {
            placer = FindObjectOfType<PlacementHelper>();
        }
    }

    public void OnGrabbed()
    {
        placer.OnGrabHabitat(tb);
    }

    public void OnReleased()
    {
        placer.OnReleaseHabitat();
    }
}
