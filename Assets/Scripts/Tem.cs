using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tem : MonoBehaviour
{
  public Camera playerCam;

  private void Start()
  {
    playerCam = Camera.main;
  }

  private void Update()
  {
    Vector3 lookDir = transform.position - playerCam.transform.position;
    if (lookDir.sqrMagnitude <= 0.0001f)
    {
      return;
    }

    transform.rotation = Quaternion.LookRotation(lookDir.normalized);
  }
}
