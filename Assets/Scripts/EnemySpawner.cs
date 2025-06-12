using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField]
    float spawnInterval = 2f; 
    [SerializeField]
    int maxEnemies = 10;
    [SerializeField]
    Vector2 spawnArea = new Vector2(10,10);


    private int currentEnemyCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies) return;

        Vector3 spawnPosition = transform.position;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
    }
}
