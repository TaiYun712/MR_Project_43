using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionSystem : MonoBehaviour
{
    public static RegionSystem instance;
    
    public MapMaker map;
    
    [Header("棲地生態點設設定")]
    public int BasiclandScore = 1;
    public int BasicwetScore = 3;
    public int AdvancedlandScore = 5;
    public int AdvancedwetScore = 7;

    //(每個棲地格 /所屬片 id)
    private readonly Dictionary<Vector2Int, int> regionOf = new Dictionary<Vector2Int, int>();
    
    //(tiles / ecoSum)
    private readonly Dictionary<int, RegionInfo> regions = new Dictionary<int, RegionInfo>();
    
    private int nextId = 1; //下一個新片的 id

    [Header("棲地外框")]
    public Color outlineColorBelow = new Color(1.0f, 0.58f, 0.0f); // （未達基礎）
    public Color outlineColorReached = Color.yellow;               // （已達基礎）

    [Header("棲地狀態UI")] 
    public RegionStatusUI_Ctrl RegionStatusUIPf;
    public Transform stateUIParents;
    public float uiHightOffset = 0.15f;
    
    //每片棲地對應一個狀態UI
    private readonly Dictionary<int, RegionStatusUI_Ctrl> regionUIMap = new Dictionary<int, RegionStatusUI_Ctrl>();
    
    
    //每片的資訊
    public class RegionInfo
    {
        public int id;  //片的識別碼
        public HashSet<Vector2Int> tiles = new HashSet<Vector2Int>(); //這片包含的所有格
        public int ecoSum = 0;  //這片總分
        public int stage = 0;  //目前階數
    }

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

    //===給 RehabSystem 迭代所有片
    public IEnumerable<RegionInfo> EnumerateRegions()
    {
        return regions.Values;
    }
    
    //===查某格屬於哪一片
    public bool TryGetRegion(Vector2Int g, out RegionInfo info)
    {
        info = null;
        if (!regionOf.TryGetValue(g,out int id))
        {
            return false;
        }

        return regions.TryGetValue(id, out info);
    }
    
    //===根據棲地種類回傳生態點數
    public int GetTileEcoScore(Vector2Int g)
    {
        if (!map.habitaAt.TryGetValue(g, out TileBehaviour tb)) { return 0; }

        if (tb.tileData == null) { return 0; }

        switch (tb.tileData.habitat)
        {
            case HabitaKind.LandBasic:
            {
                return BasiclandScore;
            }
            case HabitaKind.WetBasic:
            {
                return BasicwetScore;
            }
            case HabitaKind.LandAdvanced:
            {
                return AdvancedlandScore;
            }
            case HabitaKind.WetAdvanced:
            {
                return AdvancedwetScore;
            }
            default:
            {
                return 0;
            }

        }
    }
    
    //===拚上新棲地時呼叫更新
    public int OnHabitatPlaced(Vector2Int g)
    {
        // 收集所有鄰接已存在的 regionId
        HashSet<int> neighborIds = new HashSet<int>();
        foreach (Vector2Int n in map.Neighbors(g))
        {
            if (map.habitaAt.ContainsKey(n) && regionOf.TryGetValue(n, out int rid) && rid >= 0)
            {
                neighborIds.Add(rid);
            }
        }

        int targetId;
        if (neighborIds.Count == 0)
        {
            //若 沒鄰居 就新增一片新棲地
            targetId = nextId;
            nextId += 1;
            Debug.Log($"目前有{nextId -1}片棲地");

            RegionInfo r = new RegionInfo();
            r.id = targetId;
            regions[targetId] = r;
        }
        else
        {
            //若 有鄰居 就以第一個為主將其他相連棲地合併
            targetId = int.MaxValue;
            foreach (int id in neighborIds)
            {
                if (id < targetId)
                {
                    targetId = id;
                }
            }

            foreach (int other in neighborIds)
            {
                if (other == targetId)
                {
                    continue;
                }
                //把這格合併至這片棲地
                MergeRegion(other,targetId);
            }
        }
        
        //把這格加到 region 裡
        AssignTileToRegion(g,targetId);
        
        // 只重算這片的分數
        RecalcRegionEco(regions[targetId]);

        return targetId;
    }
    
    //===合併
    void MergeRegion(int fromId, int toId)
    {
        if(fromId == toId){return;}
        if(!regions.ContainsKey(fromId) || !regions.ContainsKey(toId)){return;}

        RegionInfo src = regions[fromId];
        RegionInfo dst = regions[toId];

        foreach (Vector2Int t in src.tiles)
        {
            regionOf[t] = toId;
            dst.tiles.Add(t);
        }

        regions.Remove(fromId);
        RecalcRegionEco(dst);

    }
    
    //===指派
    void AssignTileToRegion(Vector2Int g, int rid)
    {
        regionOf[g] = rid;
        regions[rid].tiles.Add(g);
    }
    
    //===重算
    void RecalcRegionEco(RegionInfo r)
    {
        int sum = 0;
        foreach (Vector2Int t in r.tiles)
        {
            sum += GetTileEcoScore(t);
        }

        r.ecoSum = sum;
        Debug.Log($"共有{sum}生態點數");
    }
    
    //====================棲地高亮(outline)=============================
    // 依 ecoSum 判斷該片顏色
    private Color GetRegionColor(int ecoSum, int baseEco)
    {
        return (ecoSum >= baseEco) ? outlineColorReached : outlineColorBelow;
    }

    public void ShowAllRegionsOutline(int baseEco)
    {
        Debug.Log($"[RegionSystem] ShowAllRegionsOutline() regions={regions.Count}");

        foreach (var r in regions.Values)
        {
            Debug.Log($" - region {r.id} tiles={r.tiles.Count} eco={r.ecoSum}");
            bool reached = r.ecoSum >= baseEco;
            foreach (var g in r.tiles)
            {
                if (!map.habitaAt.TryGetValue(g, out TileBehaviour tb) || tb == null) { continue; }
                OutlineShellCtrl ctrl = tb.GetComponent<OutlineShellCtrl>();
                if (ctrl == null)
                {
                    Debug.LogWarning($"   tile {g} 沒有 OutlineShellCtrl");
                    continue;
                }

                if (reached) { ctrl.ShowYellow(); }
                else { ctrl.ShowOrange(); }
            }
        }
    }

// 更新單一片
    public void RefreshOneRegionOutline(int regionId, int baseEco)
    {
        if (!regions.TryGetValue(regionId, out RegionInfo r)) { return; }
        bool reached = r.ecoSum >= baseEco;

        foreach (var g in r.tiles)
        {
            if (!map.habitaAt.TryGetValue(g, out TileBehaviour tb) || tb == null) { continue; }
            OutlineShellCtrl ctrl = tb.GetComponent<OutlineShellCtrl>();
            if (ctrl == null) { continue; }

            if (reached) { ctrl.ShowYellow(); }
            else { ctrl.ShowOrange(); }
        }
    }

// 全部隱藏
    public void HideAllRegionsOutline()
    {
        foreach (var r in regions.Values)
        {
            foreach (var g in r.tiles)
            {
                if (!map.habitaAt.TryGetValue(g, out TileBehaviour tb) || tb == null) { continue; }
                OutlineShellCtrl ctrl = tb.GetComponent<OutlineShellCtrl>();
                if (ctrl != null) { ctrl.Hide(); }
            }
        }
    }
    
    //====================棲地狀態UI=============================
    //取得該片棲地最接近中心位置
    Vector3 GetRegionUIWorldPos(RegionInfo region)
    {
        Vector3 average = Vector3.zero;
        int count = 0;

        foreach (Vector2Int tile in region.tiles)
        {
            average += map.WorldPosGrid(tile);
            count += 1;
        }

        if (count < 0)
        {
            return transform.position;
        }

        average /= count;
        float bestDist = float.MaxValue;
        Vector2Int bestTile = default;

        foreach (Vector2Int tile in region.tiles)
        {
            Vector3 tilePos = map.WorldPosGrid(tile);
            float dist = (tilePos - average).sqrMagnitude;
            if (dist < bestDist)
            {
                bestDist = dist;
                bestTile = tile;
            }
        }

        Vector3 finalPos = map.WorldPosGrid(bestTile);
        finalPos.y += uiHightOffset;
        return finalPos;
    }

    RegionStatusUI_Ctrl GetOrCreateRegionUI(int regionId)
    {
        if (regionUIMap.TryGetValue(regionId, out RegionStatusUI_Ctrl ui))
        {
            return ui;
        }

        if (RegionStatusUIPf == null)
        {
            Debug.LogWarning("RegionSystem 沒有指定 statusUiPrefab");
            return null;
        }

        RegionStatusUI_Ctrl newUi = Instantiate(RegionStatusUIPf);

        if (stateUIParents != null)
        {
            newUi.transform.SetParent(stateUIParents, false);
        }

        newUi.HideRegionStateUI();
        regionUIMap[regionId] = newUi;
        return newUi;
    }
    
    //顯示"單一片"棲地UI
    public void ShowOneRegionStateUI(int regionId,int baseEco,int expandEco)
    {
        if(!regions.TryGetValue(regionId,out RegionInfo region)){return;}

        RegionStatusUI_Ctrl ui = GetOrCreateRegionUI(regionId);
        if(ui = null){return;}

        string message = BuildRegionStatusMessage(region, baseEco, expandEco);

        ui.transform.position = GetRegionUIWorldPos(region);
        ui.SetText(message);
        ui.ShowRegionStateUI();
    }
    
    // 顯示所有棲地片的狀態文字
    public void ShowAllRegionStatusUI(int baseEco, int expandEco)
    {
        foreach (KeyValuePair<int, RegionInfo> pair in regions)
        {
            int regionId = pair.Key;
            RegionInfo region = pair.Value;

            RegionStatusUI_Ctrl ui = GetOrCreateRegionUI(regionId);
            if (ui == null)
            {
                continue;
            }

            string message = BuildRegionStatusMessage(region, baseEco, expandEco);

            ui.transform.position = GetRegionUIWorldPos(region);
            ui.SetText(message);
            ui.ShowRegionStateUI();
        }
    }

// 隱藏所有棲地片狀態文字
    public void HideAllRegionStatusUI()
    {
        foreach (KeyValuePair<int, RegionStatusUI_Ctrl> pair in regionUIMap)
        {
            if (pair.Value != null)
            {
                pair.Value.HideRegionStateUI();
            }
        }
    }

// 延遲隱藏所有棲地片狀態文字
    public void HideAllRegionStatusUIDelay(float seconds)
    {
        StartCoroutine(HideStatusUiLater(seconds));
    }

    private IEnumerator HideStatusUiLater(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        HideAllRegionStatusUI();
    }
    
    // 建立顯示文字
    private string BuildRegionStatusMessage(RegionInfo region, int baseEco, int expandEco)
    {
        int ecoSum = region.ecoSum;

        if (ecoSum < baseEco)
        {
            int needEco = baseEco - ecoSum;
            return "未達棲息標準\n還差 " + needEco + " 點生態點";
        }
        else
        {
            int stage = region.stage;
            int nextStageEco = baseEco + ((stage + 1) * expandEco);
            int needEco = nextStageEco - ecoSum;

            if (needEco < 0)
            {
                needEco = 0;
            }

            return "已達棲息標準\n目前復育階數：" + stage + "\n還差 " + needEco + " 點可升一階";
        }
    }
    
    
}
