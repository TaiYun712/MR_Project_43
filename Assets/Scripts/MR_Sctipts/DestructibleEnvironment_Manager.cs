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

    [Header("所有碎塊&被破壞碎塊")] 
    public float allSegmentsCount = 0;
    public float destoryCount = 0;
    
    [Header("碎塊破壞程度")]
    public float environmentPower = 10;
    public float environmentPowerAlertvalue = 6;

    public float normalRestoreTime;
    public float overRestoreTime_1;
    
    [SerializeField]
    float damageRatio;
    [SerializeField]
    float restoreTime;

    [Header("採集點生成")] 
    public FindSpawnPositions spawnFinder;
    public float spawnInterval;
    
    public int maxCollectionCount;
    public int minCollectionCount;
    public int currentCollectionCount;
    
    void Start()
    {
        meshSpawner.OnDestructibleMeshCreated.AddListener(SetUpDestructibleComponents);
        allSegmentsCount = 0;  //所有碎塊數量
        destoryCount = 0;  //被破壞碎塊數量
        
        environmentPower = 10;  //初始環境生命力
        restoreTime = normalRestoreTime; //初始碎塊回復時間

        currentCollectionCount = 0;
        StartCoroutine(PlantSpawnLoop());
    }

    //開始時，為每個碎塊加上Collider
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

    //摧毀碎塊
    public void DestroyMeshSegment(GameObject segment)
    {
        if (segments.Contains(segment) && currentComponent.ReservedSegment != segment)
        {
           segment.SetActive(false);
           destoryCount++;
           damageRatio = destoryCount / allSegmentsCount;
           AudioManager.instance.WallBrokenSound();

           if (environmentPower <= 9)
           {
               restoreTime = overRestoreTime_1;
           }
           else 
           {
               restoreTime = normalRestoreTime;
;           }
           StartCoroutine(RestoreSegment(segment, restoreTime));
        }
    }

    //計算破壞程度&回復碎塊
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

    //每隔一段時間生成採集點
    IEnumerator PlantSpawnLoop()
    {
        while (true)
        {
            var wait = new WaitForSeconds(spawnInterval);
            while (true)
            {
                yield return wait;

                //若是場上採集點數量足夠或環境生命力過低就不生成
                if (currentCollectionCount >= maxCollectionCount || environmentPower <= environmentPowerAlertvalue)
                {
                    Debug.Log("目前場上採集點足夠");
                    continue;
                }
                else if(currentCollectionCount <= minCollectionCount && environmentPower > environmentPowerAlertvalue)
                {
                    SpawnPlantByRoom();
                }
                    
            }
        }
    }

    void SpawnPlantByRoom()
    {
        float ratio = 1f - damageRatio;
        int targetAmount = Mathf.RoundToInt(Random.Range(3, 10) * ratio);
        currentCollectionCount += targetAmount;
        Debug.Log("生成"+targetAmount+"個採集點");
        Debug.Log("目前場上"+currentCollectionCount+"個採集點");
        
        if(targetAmount <= 0) return;

        spawnFinder.SpawnAmount = targetAmount;
        spawnFinder.SpawnLocations = FindSpawnPositions.SpawnLocation.VerticalSurfaces;
        spawnFinder.StartSpawn();

    }
    
}
