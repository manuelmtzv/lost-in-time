using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public PlayerState playerState;
    public GameObject[] enemyPrefabs;
    private readonly List<GameObject> enemySpawnPoints = new();

    public float baseSpawnRate = 2.0f;
    public float spawnRateDecreasePerKill = 0.025f;
    public float minSpawnRate = 0.95f;
    private float nextSpawnTime;
    private float nextSpawnDelay;
    private int enemiesKilledCheckpoint = 0;

    void Start()
    {
        enemySpawnPoints.AddRange(GameObject.FindGameObjectsWithTag("EnemySpawnPoint"));
        nextSpawnTime = Time.time + baseSpawnRate;
        nextSpawnDelay = baseSpawnRate;
    }

    void Update()
    {
        if (playerState.health <= 0)
        {
            return;
        }

        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            CalculateNextSpawnTime();
        }

        int enemiesKilled = playerState.enemiesKilled;
        if (enemiesKilled >= enemiesKilledCheckpoint + 10)
        {
            baseSpawnRate += 0.125f;
            enemiesKilledCheckpoint += 10;
        }
    }

    void CalculateNextSpawnTime()
    {
        float spawnRate = Mathf.Max(baseSpawnRate - (spawnRateDecreasePerKill * playerState.enemiesKilled), minSpawnRate);

        nextSpawnTime = Time.time + Mathf.Max(spawnRate, nextSpawnDelay);
    }

    void SpawnEnemy()
    {
        GameObject spawnPoint = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Count)];
        GameObject enemyPrefab = CalculateNextEnemyPrefab();

        Instantiate(enemyPrefab, spawnPoint.transform.position, Quaternion.identity);
    }

    GameObject CalculateNextEnemyPrefab()
    {
        int probability = Random.Range(0, enemyPrefabs.Length);

        return enemyPrefabs[probability];
    }
}
