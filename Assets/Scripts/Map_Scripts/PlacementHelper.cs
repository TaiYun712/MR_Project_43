using System.Collections;
using System.Collections.Generic;
using Oculus.Interaction.HandGrab;
using UnityEngine;

public class PlacementHelper : MonoBehaviour
{
    public static PlacementHelper instance;

    public float hideUITime = 2.0f;
    
    [Header("\"地圖\"偵測")]
    public MapMaker map;
    public float maxSnapDist = 0.5f; //放置偵測距離

    [Header("\"棲地\"偵測")]
    public TileBehaviour heldHabitat;
    public GameObject ghostTile;
    public Transform mapHabitatPos;

    private Vector2Int snapGrid;
    private Vector3 snapPos;
    private bool hasSnap;
    
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    void Start()
    {
        if (map == null)
        {
            map = FindObjectOfType<MapMaker>();
        }   
        
        ghostTile.SetActive(false);
    }
    
    //辨識是否可放置棲地
    public bool TryFindBestFrontierTarget(Vector3 heldWorldPos,out Vector2Int grid,out Vector3 snapWorldPos)
    {
        grid = default;
        snapWorldPos = default;
        float bestSqr = float.MaxValue;
        bool canPut = false;
        
        //檢查是否有鄰居
        bool AdjacentToOccupied(Vector2Int cell)
        {
            foreach (var n in map.Neighbors(cell))
            {
                if (map.occupied.Contains(n))
                {
                    return true;
                }
            }

            return false;
        } 

        foreach (var f in map.frontier)
        {
            if(!AdjacentToOccupied(f)){continue;} //避免懸空
            
            var wpos = map.WorldPosGrid(f);
            float sqr = (wpos - heldWorldPos).sqrMagnitude;
            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                grid = f;
                snapWorldPos = wpos;
                canPut = true;
            }
        }

        return canPut && bestSqr <= maxSnapDist * maxSnapDist;
    }
    
    void Update()
    {
        if(heldHabitat == null){return;}

        Vector3 heldTilePos = heldHabitat.transform.position;

        if (TryFindBestFrontierTarget(heldTilePos, out snapGrid, out snapPos))
        {
            hasSnap = true;
            ghostTile.transform.position = snapPos;
            ghostTile.SetActive(true);
        }
        else
        {
            hasSnap = false;
            
            ghostTile.SetActive(false);
        }
    }
    
    //拿起棲地
    public void OnGrabHabitat(TileBehaviour hb)
    {
        heldHabitat = hb;
        
        RehabSystem rehab = RehabSystem.instance;
        RegionSystem regions = RegionSystem.instance;
        if (rehab != null && regions != null)
        {
            regions.ShowAllRegionsOutline(rehab.baseEco);                 //顯示棲地狀態外框
            regions.ShowAllRegionStatusUI(rehab.baseEco,rehab.expandEco); //顯示棲地狀態UI
        }
    }
    
    //放開棲地
    public void OnReleaseHabitat()
    {
        if (heldHabitat == null)
        {
            RegionSystem.instance.HideAllRegionVisuals();
            return;
        }

        // ===== 放手但沒吸附成功 =====
        if (!hasSnap)
        {
            heldHabitat = null;
            ghostTile.SetActive(false);

            RegionSystem.instance.HideAllRegionVisuals();
            return;
        }

        // ===== 放手但該格已被占用 =====
        if (map.habitaAt.ContainsKey(snapGrid))
        {
            heldHabitat = null;
            ghostTile.SetActive(false);

            RegionSystem.instance.HideAllRegionVisuals();
            return;
        }
        
        //確定落位，寫入gridPos座標
        heldHabitat.gridPos = snapGrid;
        AudioManager.instance.PickUpHabitatSound();
        
        if (heldHabitat.tileData == null)
        {
            heldHabitat.tileData = new TileData();
        }

        map.habitaAt[snapGrid] = heldHabitat;
        heldHabitat.transform.position = snapPos;
        heldHabitat.transform.SetParent(mapHabitatPos,true);
        heldHabitat.transform.localEulerAngles = Vector3.zero;
        
        map.UpdateFrontierAfterChange(snapGrid); //更新外圍合法位置

        //落位後不可再拿
        var ctrl = map.habitaAt[snapGrid].GetComponent<Habitat_Ctrl>();
        if (ctrl != null)
        {
            ctrl.enabled = false;
            ctrl.isLock = true;
        }
        
        //解除「手上物件」狀態
        heldHabitat = null;
        hasSnap = false;
        ghostTile.SetActive(false);

        // ===== 更新 Region 與 Rehab =====
        int regionId = RegionSystem.instance.OnHabitatPlaced(snapGrid);
        RehabSystem.instance.OnMapChange();

        RehabSystem rehab = RehabSystem.instance;
        RegionSystem regions = RegionSystem.instance;

        if (rehab != null && regions != null)
        {
            // 先把舊的全部收掉，避免殘留
            regions.HideAllRegionVisuals();

            // 只顯示這次被影響的那一片
            regions.RefreshOneRegionOutline(regionId, rehab.baseEco);
            regions.ShowOneRegionStateUI(regionId, rehab.baseEco, rehab.expandEco);

            // 2 秒後一起關掉
            regions.HideAllRegionVisualsDelay(hideUITime);
        }
        
    }
}
