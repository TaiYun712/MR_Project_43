using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tem : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("habitat"))
      {
         GameManager.instance.ToResult();
      }
   }
}
