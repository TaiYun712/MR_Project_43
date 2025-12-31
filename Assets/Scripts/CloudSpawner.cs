using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
   [Header("雲 Prefabs（每個 Prefab 上要有 Cloud.cs）")]
    [SerializeField]
    private List<GameObject> cloudPrefabs = new List<GameObject>();

    [Header("生成數量")]
    [SerializeField]
    private int spawnCount = 10;

    [Header("雲要掛在哪個父物件下（不指定就用自己）")]
    [SerializeField]
    private Transform cloudParent;

    [Header("生成中心（不指定就用自己）")]
    [SerializeField]
    private Transform spawnCenter;

    [Header("是否覆蓋 Prefab 內 Cloud 的生成區域設定")]
    [SerializeField]
    private bool overrideCloudAreaSettings = true;

    [Header("生成區域設定（給 Cloud 用）")]
    [SerializeField]
    private Cloud.SpawnAreaMode spawnAreaMode = Cloud.SpawnAreaMode.BoxXZ_WithHeightRange;

    [SerializeField]
    private Vector2 heightRangeMeters = new Vector2(1.5f, 2.0f);

    [SerializeField]
    private Vector2 boxSizeXZ_Meters = new Vector2(3.0f, 3.0f);

    [SerializeField]
    private float circleRadiusMeters = 3.0f;

    [Header("啟動時就生成一次")]
    [SerializeField]
    private bool spawnOnStart = true;

    private void Start()
    {
        if (spawnOnStart == true)
        {
            SpawnOnce();
        }
    }

    public void SpawnOnce()
    {
        if (cloudPrefabs == null)
        {
            Debug.LogError("CloudSpawner：cloudPrefabs 是 null。");
            return;
        }

        if (cloudPrefabs.Count <= 0)
        {
            Debug.LogError("CloudSpawner：cloudPrefabs 沒有放任何 Prefab。");
            return;
        }

        if (cloudParent == null)
        {
            cloudParent = this.transform;
        }

        if (spawnCenter == null)
        {
            spawnCenter = this.transform;
        }

        for (int i = 0; i < spawnCount; i++)
        {
            GameObject prefab = PickRandomPrefab();
            if (prefab == null)
            {
                continue;
            }

            GameObject cloudObj = Instantiate(prefab, cloudParent);

            Cloud cloud = cloudObj.GetComponent<Cloud>();
            if (cloud == null)
            {
                Debug.LogError("CloudSpawner：某個 Prefab 沒有 Cloud.cs，已跳過。");
                Destroy(cloudObj);
                continue;
            }

            cloud.SetSpawnCenter(spawnCenter);

            if (overrideCloudAreaSettings == true)
            {
                cloud.ApplySpawnerSettings(
                    spawnAreaMode,
                    heightRangeMeters,
                    boxSizeXZ_Meters,
                    circleRadiusMeters
                );
            }

            cloud.RerollAll();
        }
    }

    private GameObject PickRandomPrefab()
    {
        int safety = 0;

        while (safety < 50)
        {
            safety++;

            int index = Random.Range(0, cloudPrefabs.Count);
            GameObject prefab = cloudPrefabs[index];

            if (prefab != null)
            {
                return prefab;
            }
        }

        Debug.LogError("CloudSpawner：cloudPrefabs 裡面可能都是空的。");
        return null;
    }
}
