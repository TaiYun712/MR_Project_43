using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;

public class DestructibleEnvironment_Manager : MonoBehaviour
{
    [Header("碎塊生成")]
    public DestructibleGlobalMeshSpawner meshSpawner;
    
    private List<GameObject> segments = new List<GameObject>();
    private DestructibleMeshComponent currentComponent;
    [SerializeField]
    private int ignoreLayer = 2;

    [Header("碎塊互動")] 
    public float environmentPower = 10;
    
    public float allSegmentsCount = 0;
    public float destoryCount = 0;
    
    [SerializeField]
    float damageRatio;
    [SerializeField]
    float restoreTime;
    void Start()
    {
        meshSpawner.OnDestructibleMeshCreated.AddListener(SetUpDestructibleComponents);
        
        allSegmentsCount = 0; 
        destoryCount = 0;
        
        environmentPower = 10;
        restoreTime = 10f;
    }

    public void SetUpDestructibleComponents(DestructibleMeshComponent component)
    {
        currentComponent = component;
        
        component.GetDestructibleMeshSegments(segments);

        foreach (var item in segments)
        {
            item.AddComponent<MeshCollider>();
            item.tag = "DestructibleWalls";
            item.layer = ignoreLayer;

            allSegmentsCount++;
        }
    }

    public void DestroyMeshSegment(GameObject segment)
    {
        if (segments.Contains(segment) && currentComponent.ReservedSegment != segment)
        {
           segment.SetActive(false);
           destoryCount++;
           damageRatio = destoryCount / allSegmentsCount;

           if (environmentPower <= 9)
           {
               restoreTime = 20f;
           }
           else 
           {
               restoreTime = 10f;
;           }
           StartCoroutine(RestoreSegment(segment, restoreTime));
        }
    }

    IEnumerator RestoreSegment(GameObject segment, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!segment.activeSelf)
        {
            segment.SetActive(true);
            destoryCount--;
            damageRatio = destoryCount / allSegmentsCount;

            if (damageRatio >= 0.1)
            {
                environmentPower--;
            }
            else
            {
                environmentPower = 10;
            }
           
            
        }
    }
}
