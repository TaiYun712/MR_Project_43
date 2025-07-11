using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("target"))
      {
         Destroy(other.gameObject);
         Destroy(this.gameObject);
         
         AudioManager.instance.TargetBroken();
      }
   }
}
