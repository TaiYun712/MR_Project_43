using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.XR.MRUtilityKit;
using UnityEngine.UI;

public class DestructibleEnvironment_Manager : MonoBehaviour
{
    [Header("場上目前數值")]
    public int environmentPower = 10;   //"目前"生命力
    public int currentDirtyCount;
    public int currentCollectionCount;
    
    [Header("所有碎塊&被破壞碎塊")] 
    public float allSegmentsCount = 0;
    public float destoryCount = 0;
    
    [Header("環境破壞程度/環境生命力")]
    
    public int maxEnvironmentPower = 10;//"最大"生命力
    
    public float normalRestoreTime;       //一般回復時間
    public float overRestoreTime_1;       //"過度時"回復時間

    public int newSpawnCount;
    public int normalMaxSpwanCount = 10;
    public int maxSpwanCount_1 = 8; //等級1破壞_採集點生成數
    public int maxSpwanCount_2 = 5; //等級1破壞_採集點生成數
    
    [Header("環境生命力警示")]
   
    public int environmentWarning_1 = 8;
    public int environmentWarning_2 = 6;
    public int environmentWarning_3 = 5;
    private bool hasAlerted = false;
    
    [SerializeField]
    float restoreTime;

    [Header("碎塊破壞等級(比例)")]
    public float damageTier1 = 0.10f;  // 10% 以上 > 等級1
    public float damageTier2 = 0.30f;  // 30% 以上 > 等級2
    public float damageTier3 = 0.50f;  // 50% 以上 > 等級3
    
    [Header("髒污汙染等級（數量）")]
    public int dirtyLevel1 = 5;        // 髒污≥5 > 等級1
    public int dirtyLevel2 = 10;       // 髒污≥10 > 等級2
    public int dirtyLevel3 = 15;       // 髒污≥15 > 等級3
    
    [Header("碎塊生成")]
    public DestructibleGlobalMeshSpawner meshSpawner;
    
    private List<GameObject> segments = new List<GameObject>();
    private DestructibleMeshComponent currentComponent;
    [SerializeField]
    private int ignoreLayer = 2;
    
    [Header("採集點生成")] 
    public FindSpawnPositions spawnFinder;
    public Transform holeSpawnPos;
    public float spawnInterval;
    
    public int maxCollectionCount;
    public int minCollectionCount;
    
    public GameObject[] collectHolePfs;

    [Header("髒污生成")] 
    public FindSpawnPositions dirtySpawnFinder;
    public Transform dirtySpawnPos;
    public float dirtySpawmInterval;
    public int maxDirtyCount;
    public GameObject[] dirtyPfs;
    
    [Header("警示控制")]
    [SerializeField] bool hasDamageWarned = false;
    [SerializeField] private bool hasDirtyWarned  = false;

    [Header("中文顯示文本")]
    public string overdestructionHint_CN;
    public string overpollutionHint_CN;
    
    [Header("英文顯示文本")]
    [TextArea(2,5)]
    public string overdestructionHint_EN;
    [TextArea(2,5)]
    public string overpollutionHint_EN;
    void Start()
    {
        meshSpawner.OnDestructibleMeshCreated.AddListener(SetUpDestructibleComponents);
        allSegmentsCount = 0;  //所有碎塊數量
        destoryCount = 0;  //被破壞碎塊數量
        
        environmentPower = 10;  //初始環境生命力
        restoreTime = normalRestoreTime; //初始碎塊回復時間

        currentCollectionCount = 0;
        StartCoroutine(PlantSpawnLoop());
        StartCoroutine(DirtySpawnLoop());
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
           AudioManager.instance.WallBrokenSound();

           destoryCount = destoryCount + 1;
           if (destoryCount > allSegmentsCount) { destoryCount = allSegmentsCount;}
           ReComputeEnviromentPower();
         
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

            destoryCount = destoryCount - 1;
            if (destoryCount < 0) { destoryCount = 0; }
            ReComputeEnviromentPower();

        }
    }

    //每隔一段時間生成採集點
    IEnumerator PlantSpawnLoop()
    {
       var wait = new WaitForSeconds(spawnInterval);
        while (true)
        {
             yield return wait;

             //不是遊戲階段就跳過
             if (GameManager.instance == null || !GameManager.instance.isPlaying) { continue; }
             
             ReComputeEnviromentPower();
             
            //若是場上採集點數量足夠或環境生命力過低就不生成
             if (currentCollectionCount >= maxCollectionCount)
             {
                 Debug.Log("目前場上採集點足夠");
                 continue;
             }
             else if(environmentPower <= environmentWarning_3)
             {
                 Debug.Log("環境生命力過低，不給生");
                 continue;
             }
             else if(currentCollectionCount <= minCollectionCount && environmentPower > environmentWarning_3)
             {
                    SpawnPlantByRoom();
             }
                    
        }
    }

    void SpawnPlantByRoom()
    {
        ReComputeEnviromentPower();
        int targetAmount= Mathf.RoundToInt(Random.Range(3, newSpawnCount));
        if(targetAmount <= 0) return;
        Debug.Log("嘗試生成"+targetAmount+"個採集點");
        
        int index = Random.Range(0, collectHolePfs.Length);
        spawnFinder.SpawnObject = collectHolePfs[index];
        spawnFinder.SpawnAmount = targetAmount;
        spawnFinder.SpawnLocations = FindSpawnPositions.SpawnLocation.VerticalSurfaces;
        spawnFinder.StartSpawn();

        currentCollectionCount = holeSpawnPos.childCount;
        Debug.Log("目前場上"+currentCollectionCount+"個採集點"); 

    }

    //每隔一段時間生成髒污
    IEnumerator DirtySpawnLoop()
    {
        var wait = new WaitForSeconds(dirtySpawmInterval);
        ReComputeEnviromentPower();
        while (true)
        {
            yield return wait;
            
            //不是遊戲階段就跳過
            if (GameManager.instance == null || !GameManager.instance.isPlaying) { continue; }
            
            if (currentDirtyCount < maxDirtyCount )
            {
                SpawnDirtyByRoom();
            }
        }
    }

    void SpawnDirtyByRoom()
    {
        int dirtyAmount;
        int dirtySpawnAmount;
        dirtyAmount = Random.Range(0,12);
        
        if (dirtyAmount % 2 == 0)   //偶數才生，奇數不生
        {
            dirtySpawnAmount = dirtyAmount;
            Debug.Log($"嘗試生成{dirtySpawnAmount}個髒污");
        }
        else
        {
            dirtySpawnAmount = 0;
            Debug.Log($"抽中奇數，不生成髒污");
        }
        
        int index = Random.Range(0, dirtyPfs.Length);
        dirtySpawnFinder.SpawnObject = dirtyPfs[index];
        dirtySpawnFinder.SpawnAmount = dirtySpawnAmount;
        dirtySpawnFinder.SpawnLocations = FindSpawnPositions.SpawnLocation.VerticalSurfaces;
        dirtySpawnFinder.StartSpawn();

        currentDirtyCount = dirtySpawnPos.childCount;
        Debug.Log("目前場上"+currentDirtyCount+"個髒污"); 
    }

    
    //根據破壞計算環境生命力
    void ReComputeEnviromentPower()
    {
        int damageTier = 0;
        float  damageRatio = 0f;

        if (allSegmentsCount > 0)
        {
            damageRatio = (float)destoryCount / (float)allSegmentsCount;
        }
        else
        {
            damageRatio = 0;
        }
        
        //碎塊破壞程度
        if (damageRatio >= damageTier3) { damageTier = 3; }
        else if (damageRatio >= damageTier2) { damageTier = 2; }
        else if (damageRatio >= damageTier1) { damageTier = 1; }
        else { damageTier = 0; }

        //髒污汙染程度
        int dirtyTier = 0;
        
        if (currentDirtyCount >= dirtyLevel3) { dirtyTier = 6; }
        else if(currentDirtyCount >= dirtyLevel2) { dirtyTier = 4; }
        else if (currentDirtyCount >= dirtyLevel1) { dirtyTier = 2;}
        else { dirtyTier = 0; }

        //提示破壞狀況
        if (damageTier >= 2 && !hasDamageWarned)
        {
            Debug.Log("環境破碎嚴重");
            if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
            {
                 PanelUI_Ctrl.instance.OverDestoryWarningHint(overdestructionHint_CN);
            }
            else
            {
                PanelUI_Ctrl.instance.OverDestoryWarningHint(overdestructionHint_EN);
            }
          
        }
        else
        {
            hasDamageWarned = false;
        }

        if (dirtyTier >= 4 && !hasDirtyWarned)
        {
            Debug.Log("環境污染過多");
            
            if (LanguageManager.instance.currentLanguage == LanguageManager.GameLanguage.Chinese)
            {
                PanelUI_Ctrl.instance.OverDestoryWarningHint(overpollutionHint_CN);
            }
            else
            {
                PanelUI_Ctrl.instance.OverDestoryWarningHint(overpollutionHint_EN);
            }
            
            hasDirtyWarned = true;
        }
        else
        {
            hasDirtyWarned = false;
        }
      //計算總破壞程度  
        int totalDamage = damageTier + dirtyTier;
        int newPower = maxEnvironmentPower - totalDamage;
        
        //避免低於0或高於上限
        if (newPower < 0) { newPower = 0; }
        if (newPower > maxEnvironmentPower) { newPower = maxEnvironmentPower; }
        if (environmentPower < 0) { environmentPower = 0; }

        environmentPower = newPower;
        
        //根據破壞程度調整回復時間
        if (environmentPower <= environmentWarning_2)
        {
            restoreTime = overRestoreTime_1;
            Debug.Log("現在回復時間" + restoreTime);
        }
        else
        {
            restoreTime = normalRestoreTime;
            Debug.Log("現在回復時間" + restoreTime);
        }
        
        //根據破壞程度調整採集點生成
        if (environmentPower <= environmentWarning_1) //8
        {
            newSpawnCount = maxSpwanCount_1; //8
            Debug.Log("破壞程度1，最多生" + maxSpwanCount_1);
        }
        else if (environmentPower <= environmentWarning_2) //6
        {
            newSpawnCount = maxSpwanCount_2; //5
            Debug.Log("破壞程度2，最多生" + maxSpwanCount_2);
        }
        else 
        {
            newSpawnCount = normalMaxSpwanCount; //10
            Debug.Log("破壞程度小，生最多" + normalMaxSpwanCount);
        }
    }

 
    
}
