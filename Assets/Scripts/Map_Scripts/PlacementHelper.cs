using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementHelper : MonoBehaviour
{
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

        foreach (var f in map.frontier)
        {
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
    }
    
    //放開棲地
    public void OnReleaseHabitat()
    {
        if(heldHabitat == null){return;}
        if(!hasSnap){return;}  //放在合法範圍外的隨意位置
        if(map.habitaAt.ContainsKey(snapGrid)){return;} //檢查位置是否被占用

        //確定落位，寫入gridPos座標
        heldHabitat.gridPos = snapGrid;

        if (heldHabitat.tileData == null)
        {
            heldHabitat.tileData = new TileData();
        }

        map.habitaAt[snapGrid] = heldHabitat;
        heldHabitat.transform.position = snapPos;
        heldHabitat.transform.SetParent(mapHabitatPos,true);
        heldHabitat.transform.localEulerAngles = Vector3.zero;
        
        map.UpdateFrontierAfterChange(snapGrid); //更新外圍合法位置

        //解除「手上物件」狀態
        heldHabitat = null;
        hasSnap = false;
        ghostTile.SetActive(false);

    }
}
