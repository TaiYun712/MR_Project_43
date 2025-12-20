using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class TestBullet : MonoBehaviour
{
   public DestructibleEnvironment_Manager destructibleManager;
   public PlantManager plantManager;

   public ParticleSystem[] dirtDestoryPts;
   
   private void Start()
   {
      destructibleManager = FindObjectOfType<DestructibleEnvironment_Manager>();
      plantManager = FindObjectOfType<PlantManager>();
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("target"))
      {
         Destroy(other.gameObject);
         destructibleManager.currentCollectionCount--;
         //Debug.Log("已採集，目前場上採集點為" + destructibleManager.currentCollectionCount);
         ReturnToPool();
         
         plantManager.GetPlant();
         AudioManager.instance.TargetBroken();
      }
      else
      {
         if (other.CompareTag("DestructibleWalls"))
         {
            int destoryPrtIndex = Random.Range(0, dirtDestoryPts.Length);
            destructibleManager.DestroyMeshSegment(other.gameObject);
            dirtDestoryPts[destoryPrtIndex].Play();
            
            Invoke(nameof(ReturnToPool),0.2f);
         }
         
      }
   }

   private void ReturnToPool()
   {
      BulletPool.instance.ReturnBullet(this.gameObject);
   }
}
