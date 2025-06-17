using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemyPrefab;
    [SerializeField] float spawnInterval = 2f; 
    [SerializeField] int maxEnemies = 10;

    private int currentEnemyCount = 0;

    void Start()
    {
        InvokeRepeating(nameof(SpawnEnemy), 0f, spawnInterval);
        enemyPrefab.tag = "Enemy";
    }

    void SpawnEnemy()
    {
        if (currentEnemyCount >= maxEnemies) return;

        Vector2 spawnPosition = transform.position;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        currentEnemyCount++;
    }
}
