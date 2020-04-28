using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSpace;

public class MapMgr : MonoBehaviour
{
    public GameObject[] OutWallArr;
    public GameObject[] FloorArr;
    public GameObject[] WallArr;
    public GameObject[] FoodArr;
    public GameObject[] EnemyArr;
    public GameObject ExitPrefab;
    public GameObject PlayerPrefab;

    GameObject Map;

    public int MinCountWall = 2;
    public int MaxCountWall = 8;

    void Start()
    {
        initMap(Game.Instance.rows, Game.Instance.cols);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void initMap (int rows, int cols)
    {
        float rowsMid = Game.Instance.m_rowMid;
        float colsMid = Game.Instance.m_colMid;

        if (Map) GameObject.Destroy(Map);

        Map = new GameObject("Map");
        // 生成外墙与地板
        for (int x = 0; x < cols; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                if (x == 0 || y == 0 || x == cols - 1 || y == rows - 1)
                {
                    GameObject outWallPrefab = RandomPrefab(OutWallArr);
                    GameObject go = GameObject.Instantiate(outWallPrefab, new Vector3(x - colsMid, y - rowsMid, 0), Quaternion.identity);
                    go.transform.SetParent(Map.transform);
                }
                else
                {
                    GameObject floorPrefab = RandomPrefab(FloorArr);
                    GameObject go = GameObject.Instantiate(floorPrefab, new Vector3(x - colsMid, y - rowsMid, 0), Quaternion.identity);
                    go.transform.SetParent(Map.transform);
                }
            }
        }

        // 生成障碍物位置列表
        List<Vector2> positionList = new List<Vector2>();


        for (int x = 2; x < cols - 2; x++)
        {
            for (int y = 2; y < rows - 2; y++)
            {
                positionList.Add(new Vector2(x - colsMid, y - rowsMid));
            }
        }
        // 随机障碍物个数
        int countWall = Random.Range(MinCountWall, MaxCountWall + 1);
        InstantiateItems(countWall, positionList, WallArr);

        // 生成食物 食物的数量 2 - level * 2;
        int foodCount = Random.Range(2, Game.Instance.Level + 1);
        InstantiateItems(foodCount, positionList, FoodArr);

        // 生成玩家
        GameObject player = GameObject.Instantiate(PlayerPrefab, new Vector3(1 - colsMid, 1 - rowsMid, 0), Quaternion.identity);
        player.transform.SetParent(Map.transform);

        // 生成敌人
        int enemyCount = Game.Instance.Level / 2;
        List<GameObject> enemys = InstantiateItems(enemyCount, positionList, EnemyArr);
        enemys.ForEach((GameObject enemy) =>
        {
            Enemy enemySC = enemy.GetComponent<Enemy>();
            enemySC.SetPlayer(player);
        });

        // 退出位置
        GameObject ExitGO = GameObject.Instantiate(ExitPrefab, Game.Instance.m_exitVector, Quaternion.identity);
        ExitGO.transform.SetParent(Map.transform);

        Game.Instance.InitGame();
    }

    List<GameObject> InstantiateItems (int count, List<Vector2> positionList, GameObject[] Prefabs)
    {
        List<GameObject> res = new List<GameObject>();
        for (int i = 0; i < count && positionList.Count > 0; i++)
        {
            Vector2 pos = RandomPosition(ref positionList);
            GameObject prefab = RandomPrefab(Prefabs);
            GameObject go = GameObject.Instantiate(prefab, new Vector3(pos.x, pos.y), Quaternion.identity);
            go.transform.SetParent(Map.transform);
            res.Add(go);
        }
        return res;
    }
    Vector2 RandomPosition(ref List<Vector2> positions)
    {
        int pIdx = Random.Range(0, positions.Count);
        Vector2 pos = positions[pIdx]; 
        positions.RemoveAt(pIdx);
        return pos;
    }
    GameObject RandomPrefab(GameObject[] Prefabs)
    {
        int rIdx = Random.Range(0, Prefabs.Length);
        return Prefabs[rIdx];
    }
}
