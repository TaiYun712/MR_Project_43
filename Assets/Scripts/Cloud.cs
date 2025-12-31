using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
   public enum SpawnAreaMode
    {
        BoxXZ_WithHeightRange,
        CircleXZ_WithHeightRange
    }

    [Header("生成區域模式")]
    [SerializeField]
    private SpawnAreaMode spawnAreaMode = SpawnAreaMode.BoxXZ_WithHeightRange;

    [Header("生成中心 (不指定就用自己的 Transform)")]
    [SerializeField]
    private Transform spawnCenter;

    [Header("高度範圍 (以 spawnCenter 的 Y 為基準，加上這個範圍)")]
    [SerializeField]
    private Vector2 heightRangeMeters = new Vector2(1.5f, 2.0f);

    [Header("模式A：盒狀區域 (只用 XZ，單位公尺)")]
    [SerializeField]
    private Vector2 boxSizeXZ_Meters = new Vector2(3.0f, 3.0f);

    [Header("模式B：圓形區域半徑 (只用 XZ，單位公尺)")]
    [SerializeField]
    private float circleRadiusMeters = 3.0f;

    [Header("漂移方向/速度")]
    [Tooltip("速度範圍 (公尺/秒)")]
    [SerializeField]
    private Vector2 driftSpeedRange_MetersPerSecond = new Vector2(0.03f, 0.12f);

    [Tooltip("是否只在水平 XZ 平面漂移（建議開）")]
    [SerializeField]
    private bool driftOnlyOnXZ = true;

    [Header("定時重新隨機")]
    [Tooltip("每隔幾秒重隨機一次「位置」(固定值)")]
    [SerializeField]
    private float rerollPositionIntervalSeconds = 8.0f;

    [Tooltip("每隔幾秒重隨機一次「方向/速度」(固定值)")]
    [SerializeField]
    private float rerollMoveIntervalSeconds = 4.0f;

    [Tooltip("是否在開始時立刻隨機一次")]
    [SerializeField]
    private bool rerollOnStart = true;

    [Header("外觀隨機")]
    [SerializeField]
    private bool randomYawRotation = true;

    [SerializeField]
    private bool randomScale = false;

    [SerializeField]
    private Vector2 randomScaleRange = new Vector2(0.9f, 1.1f);

    private Vector3 driftDirectionWorld;
    private float driftSpeed;
    private float positionTimer;
    private float moveTimer;

    private void Start()
    {
        if (spawnCenter == null)
        {
            spawnCenter = this.transform;
        }

        if (rerollOnStart == true)
        {
            RerollAll();
        }
        else
        {
            // 至少也要有漂移方向/速度
            RerollMove();
        }
    }

    private void Update()
    {
        // 平移
        Vector3 delta = driftDirectionWorld * driftSpeed * Time.deltaTime;
        transform.position = transform.position + delta;

        // 計時器
        positionTimer += Time.deltaTime;
        moveTimer += Time.deltaTime;

        // 重隨機位置
        if (rerollPositionIntervalSeconds > 0.0f)
        {
            if (positionTimer >= rerollPositionIntervalSeconds)
            {
                positionTimer = 0.0f;
                RerollPosition();
            }
        }

        // 重隨機移動
        if (rerollMoveIntervalSeconds > 0.0f)
        {
            if (moveTimer >= rerollMoveIntervalSeconds)
            {
                moveTimer = 0.0f;
                RerollMove();
            }
        }
    }

    public void SetSpawnCenter(Transform newCenter)
    {
        if (newCenter == null)
        {
            return;
        }

        spawnCenter = newCenter;
    }

    public void ApplySpawnerSettings(
        SpawnAreaMode newMode,
        Vector2 newHeightRangeMeters,
        Vector2 newBoxSizeXZ_Meters,
        float newCircleRadiusMeters
    )
    {
        spawnAreaMode = newMode;
        heightRangeMeters = newHeightRangeMeters;
        boxSizeXZ_Meters = newBoxSizeXZ_Meters;
        circleRadiusMeters = newCircleRadiusMeters;
    }

    public void RerollAll()
    {
        positionTimer = 0.0f;
        moveTimer = 0.0f;

        RerollPosition();
        RerollMove();
        RerollLook();
    }

    public void RerollPosition()
    {
        if (spawnCenter == null)
        {
            spawnCenter = this.transform;
        }

        Vector3 centerPos = spawnCenter.position;

        float yOffset = Random.Range(heightRangeMeters.x, heightRangeMeters.y);
        float y = centerPos.y + yOffset;

        if (spawnAreaMode == SpawnAreaMode.BoxXZ_WithHeightRange)
        {
            float halfX = boxSizeXZ_Meters.x * 0.5f;
            float halfZ = boxSizeXZ_Meters.y * 0.5f;

            float x = centerPos.x + Random.Range(-halfX, halfX);
            float z = centerPos.z + Random.Range(-halfZ, halfZ);

            transform.position = new Vector3(x, y, z);
            return;
        }

        if (spawnAreaMode == SpawnAreaMode.CircleXZ_WithHeightRange)
        {
            Vector2 inside = Random.insideUnitCircle * circleRadiusMeters;

            float x = centerPos.x + inside.x;
            float z = centerPos.z + inside.y;

            transform.position = new Vector3(x, y, z);
            return;
        }
    }

    public void RerollMove()
    {
        // 隨機方向（以角度最直觀）
        float angle = Random.Range(0.0f, 360.0f);
        float rad = angle * Mathf.Deg2Rad;

        float x = Mathf.Cos(rad);
        float z = Mathf.Sin(rad);

        Vector3 dir = new Vector3(x, 0.0f, z);
        dir = dir.normalized;

        if (driftOnlyOnXZ == false)
        {
            // 如果你想要有一點點上下飄
            float y = Random.Range(-0.08f, 0.08f);
            dir = new Vector3(dir.x, y, dir.z);
            dir = dir.normalized;
        }

        driftDirectionWorld = dir;

        driftSpeed = Random.Range(driftSpeedRange_MetersPerSecond.x, driftSpeedRange_MetersPerSecond.y);
    }

    public void RerollLook()
    {
        if (randomYawRotation == true)
        {
            Vector3 euler = transform.eulerAngles;
            euler.y = Random.Range(0.0f, 360.0f);
            transform.eulerAngles = euler;
        }

        if (randomScale == true)
        {
            float s = Random.Range(randomScaleRange.x, randomScaleRange.y);
            transform.localScale = new Vector3(s, s, s);
        }
    }
}
