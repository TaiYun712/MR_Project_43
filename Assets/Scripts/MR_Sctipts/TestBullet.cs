using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
   public DestructibleEnvironment_Manager destructibleManager;

   private void Start()
   {
      destructibleManager = FindObjectOfType<DestructibleEnvironment_Manager>();
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("target"))
      {
         Destroy(other.gameObject);
         Destroy(this.gameObject);
         
         AudioManager.instance.TargetBroken();
      }
      else
      {
         if (other.CompareTag("DestructibleWalls"))
         {
            destructibleManager.DestroyMeshSegment(other.gameObject);
            Destroy(this.gameObject);
         }
         
      }
      
   }
}
