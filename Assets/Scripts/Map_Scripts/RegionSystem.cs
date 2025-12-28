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
            if (map.habitaAt.ContainsKey(n) && regionOf.TryGetValue(n, out int rid) && rid > 0)
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

            RegionInfo r = new RegionInfo();
            r.id = targetId;
            regions[targetId] = r;
        }
        else
        {
            //若 有鄰居 就以第一個為主將其他相連棲地合併
            using (var it = neighborIds.GetEnumerator())
            {
                it.MoveNext();
                targetId = it.Current;
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
    
    
}
