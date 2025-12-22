using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    [Header("References")]
    public MapGenerator mapGenerator;
    public TilePool tilePool;

    [Header("Setting")]
    public float tileSize = 1.0f;

    readonly List<GameObject> activeTiles = new List<GameObject>();
    Vector2 hexCoords;

    //對外公開目前的地圖資料與 TileBehaviour 查表
    public TileData[,] MapData { get; private set; }
    public TileBehaviour[,] GridBehaviours { get; private set; }
    
    //佔用集合與外圈集合（合法拼接位置）
    public HashSet<Vector2Int> occupied = new HashSet<Vector2Int>();
    public HashSet<Vector2Int> frontier = new HashSet<Vector2Int>();
    
    //目前"已放上"的棲地
    public Dictionary<Vector2Int, TileBehaviour> habitaAt = new Dictionary<Vector2Int, TileBehaviour>();


    void Start()
    {
        MakeMap();
    }

    Vector2 GetHexCoords(int x, int y) // 調整六邊形的連接位置
    {
        float xPos = x * tileSize * Mathf.Cos(Mathf.Deg2Rad * 30);
        float yPos = y * tileSize + ((x % 2 == 1) ? tileSize * 0.5f : 0);
        return new Vector2(xPos, yPos);
    }
    
    //給外部拿取的世界座標
    public Vector3 WorldPosGrid(Vector2Int g)
    {
        var v2 = GetHexCoords(g.x, g.y);
        return transform.TransformPoint(new Vector3(v2.x, 0f, v2.y));
    }

    //生成地圖
    public void MakeMap()
    {
        ClearTiles();
        
        MapData = mapGenerator.GenerateMapData();
        int width = MapData.GetLength(0);
        int height = MapData.GetLength(1);
        
        GridBehaviours = new TileBehaviour[width, height]; //新增

      
       //生成tile
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
               TileData data = MapData[x, y];
               if(data == null) { continue; }

                //從物件池中取tile
                GameObject tileGO = tilePool.GetTile(data);
                if (tileGO == null) //沒資料的跳過
                {
                    Debug.Log(data + "tile生成失敗");
                    continue;
                }
               

                tileGO.transform.SetParent(this.transform,false);
                hexCoords = GetHexCoords(x,y);
                tileGO.transform.localPosition = new Vector3(hexCoords.x, 0, hexCoords.y);
                activeTiles.Add(tileGO);

                var behavior = tileGO.GetComponent<TileBehaviour>();
                if( behavior != null)
                {
                    behavior.gridPos = new Vector2Int(x, y);
                    behavior.tileData = data;
                    GridBehaviours[x, y] = behavior;
                }
            }
        }
        
        ////=====  生成外圈  =====
        BuildOccupyAndFrontier();

    }

    public void ClearTiles()
    {
        foreach (var tileGo in activeTiles)
        {
            var behaviour = tileGo.GetComponent<TileBehaviour>();
            if (behaviour == null || behaviour.tileData == null) { continue; }
            tilePool.ReturnTile(tileGo, behaviour.tileData);
        }
        activeTiles.Clear();
        
        occupied.Clear();
        frontier.Clear();
    }
    
    //===== 鄰居 =====
    
    // ===== Axial <-> Offset(odd-q) 轉換 =====
    private Vector2Int OffsetOddQ_ToAxial(Vector2Int p)
    {
        // q = x
        // r = y - (x - (x & 1)) / 2
        int q = p.x;
        int r = p.y - (p.x - (p.x & 1)) / 2;
        return new Vector2Int(q, r);
    }

    private Vector2Int Axial_ToOffsetOddQ(Vector2Int a)
    {
        // x = q
        // y = r + (q - (q & 1)) / 2
        int x = a.x;
        int y = a.y + (a.x - (a.x & 1)) / 2;
        return new Vector2Int(x, y);
    }

// 六個軸向鄰居（axial 座標系）
    private static readonly Vector2Int[] axialDirs = new Vector2Int[]
    {
        new Vector2Int(+1,  0),
        new Vector2Int(+1, -1),
        new Vector2Int( 0, -1),
        new Vector2Int(-1,  0),
        new Vector2Int(-1, +1),
        new Vector2Int( 0, +1)
    };

    public IEnumerable<Vector2Int> Neighbors(Vector2Int p)
    {
        if (MapData == null)
        {
            yield break;
        }

        int w = MapData.GetLength(0);
        int h = MapData.GetLength(1);

        // 1) 先把 offset(odd-q) 轉 axial
        Vector2Int a = OffsetOddQ_ToAxial(p);

        // 2) 在 axial 走 6 個方向，再轉回 offset(odd-q)
        for (int i = 0; i < axialDirs.Length; i++)
        {
            Vector2Int na = new Vector2Int(a.x + axialDirs[i].x, a.y + axialDirs[i].y);
            Vector2Int q = Axial_ToOffsetOddQ(na);

            if (q.x >= 0 && q.y >= 0 && q.x < w && q.y < h)
            {
                yield return q;
            }
        }
    }
    
    /*
    private static readonly Vector2Int[] offodd =
        { new(+1, 0), new(0, +1), new(-1, +1), new(-1, 0), new(-1, -1), new(0, -1) };

    private static readonly Vector2Int[] offEven = 
        {  new(+1,0), new(+1,+1), new(0,+1), new(-1,0), new(0,-1), new(+1,-1)  };

    public IEnumerable<Vector2Int> Neighbors(Vector2Int p)
    {
        if(MapData == null){yield break;}

        int w = MapData.GetLength(0), h = MapData.GetLength(1);
        var offs = (p.x % 2 == 1) ? offodd : offEven;
        foreach (var d in offs)
        {
            var q = p + d;
            if (q.x >= 0 && q.y >= 0 && q.x < w && q.y < h)
            {
                yield return q;
            }
        }
    }
    */
    
    //生成地圖外圈
    void BuildOccupyAndFrontier()
    {
        occupied.Clear();
        frontier.Clear();

        int w = MapData.GetLength(0), h = MapData.GetLength(1);
        
        // 1) 由讀圖生的底塊加入佔用
        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                if (MapData[x,y] !=null)
                {
                    occupied.Add(new Vector2Int(x, y));
                }
            }
        }

        // 2) 已放上的棲地也算佔用
        foreach (var kv in habitaAt)
        {
            occupied.Add(kv.Key);
        }
        
        // 3) 外圈 = 任何「鄰到佔用、但自己沒被佔用且不在 MapData 內」的位置
        foreach (var p in occupied)
        {
            foreach (var n in Neighbors(p) )
            {
                if (!occupied.Contains(n) && MapData[n.x, n.y] == null)
                {
                    frontier.Add(n);
                }
            }
        }
        
    }
    
    // 重新計算與 g 相鄰的外圈（局部更新）

    bool InBounds(Vector2Int q)
    {
        int w = MapData.GetLength(0), h = MapData.GetLength(1);
        return q.x >= 0 && q.y >= 0 && q.x < w && q.y < h;
    }
    
    public void UpdateFrontierAfterChange(Vector2Int g)
    {
        occupied.Add(g);
        frontier.Remove(g);

        var toCheck = new HashSet<Vector2Int>() { g };
        foreach (var n in Neighbors(g))
        {
            toCheck.Add(n);
            foreach (var m in Neighbors(n))
            {
                toCheck.Add(m);
            }
        }

        foreach (var c in toCheck)
        {
            if(!InBounds(c)){continue;}

            frontier.Remove(c);

            if (!occupied.Contains(c) && MapData[c.x, c.y] == null)
            {
                bool nearOcc = false;

                foreach (var n in Neighbors(c))
                {
                    if (occupied.Contains(n))
                    {
                        nearOcc = true;
                        break;
                    }
                }

                if (nearOcc)
                {
                    frontier.Add(c);
                }
            }
        }
        
    }
    
}
    
      
    
