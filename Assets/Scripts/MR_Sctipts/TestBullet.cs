using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
   public DestructibleEnvironment_Manager destructibleManager;
   public Demo_GetPlant getPlant;

   public ParticleSystem dirtDestoryPt;
   
   private void Start()
   {
      destructibleManager = FindObjectOfType<DestructibleEnvironment_Manager>();
      getPlant = FindObjectOfType<Demo_GetPlant>();
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("target"))
      {
         Destroy(other.gameObject);
         Destroy(this.gameObject);
         
         getPlant.ShowPlantPanel();
         AudioManager.instance.TargetBroken();
      }
      else
      {
         if (other.CompareTag("DestructibleWalls"))
         {
            destructibleManager.DestroyMeshSegment(other.gameObject);
            dirtDestoryPt.Play();
            
            //Destroy(this.gameObject,0.2f);
            Invoke(nameof(ReturnToPool),0.2f);
         }
         
      }
   }

   private void ReturnToPool()
   {
      BulletPool.instance.ReturnBullet(this.gameObject);
   }
}
