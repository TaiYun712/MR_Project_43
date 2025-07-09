using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpawner : MonoBehaviour
{
    public Transform playerPos;
    public GameObject soulPf;
    public int soulCount;
    public float spawnRadius;
    
    
    void Start()
    {
        SpawnSouls(soulCount);
    }

    public void SpawnSouls(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomPos = playerPos.position + Random.insideUnitSphere * spawnRadius;
            randomPos.y = Random.Range(0,1.5f);
            GameObject soul = Instantiate(soulPf, randomPos, Quaternion.identity);
            soul.GetComponent<SoulFloating>().spawner = this;
        }
    }

    public void RespawnSoul()
    {
        Vector3 randomPos = playerPos.position + Random.insideUnitSphere * spawnRadius;
        randomPos.y = Random.Range(0,1.5f);
        GameObject soul = Instantiate(soulPf, randomPos, Quaternion.identity);
        soul.GetComponent<SoulFloating>().spawner = this;

    }
}
