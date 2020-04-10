using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PointSpawners : MonoBehaviour
{
    private const string SPAWN_ENEMY_METHOD = "SpawnOneEnemy";
    private const string ENEMY_PARENT = "EnemyParent";

    private IList<SpawnPoint> spawnPoints;
    private Stack<SpawnPoint> spawnStack;
    private GameObject enemyParent;
    private WaveConfig waveConfig;

    // Event for telling the system an Enemy has spawned
    public delegate void EnemySpawned();
    public static event EnemySpawned EnemySpawnedEvent;

    void Start()
    {
        enemyParent = GameObject.Find(ENEMY_PARENT);

        if (!enemyParent)
        {
            enemyParent = new GameObject(ENEMY_PARENT);
        }

        // Get the SpawnPoints here
        spawnPoints = GetComponentsInChildren<SpawnPoint>();

        // Create a stack of SpawnPoints
        spawnStack = ListUtils.CreateShuffledStack(spawnPoints);
    }

    private void SpawnOneEnemy()
    {
        if (spawnStack.Count == 0)
        {
            spawnStack = ListUtils.CreateShuffledStack(spawnPoints);
        }

        var enemyPrefabs = waveConfig.GetEnemies();
        var enemy = Instantiate(
            enemyPrefabs[Random.Range(0, enemyPrefabs.Count)],
            enemyParent.transform
        );

        var sp = spawnStack.Pop();
        enemy.transform.position = sp.transform.position;

        if (enemy.tag == Asteroid.TAG_NAME)
        {
            SetAsteroidDirection(sp, enemy.GetComponent<AsteroidMovement>());
        }

        PublishOnEnemySpawnedEvent();
    }

    private void SetAsteroidDirection(SpawnPoint sp, AsteroidMovement asteroid)
    {
        float xDirection, yDirection;

        switch (sp.tag)
        {
            case SpawnPoint.TOP:
                // Move downwards
                xDirection = Random.Range(-1, 1);
                yDirection = -1;
                break;
            case SpawnPoint.BOTTOM:
                // Move upwards
                xDirection = Random.Range(-1, 1);
                yDirection = 1;
                break;
            case SpawnPoint.LEFT:
                // Move right
                xDirection = 1;
                yDirection = Random.Range(-1, 1);
                break;
            case SpawnPoint.RIGHT:
                // Move left
                xDirection = -1;
                yDirection = Random.Range(-1, 1);
                break;
            default:
                xDirection = Random.Range(-1, 1);
                yDirection = Random.Range(-1, 1);
                break;
        }

        asteroid.Move(new Vector2(xDirection, yDirection));
    }

    public void PublishOnEnemySpawnedEvent()
    {
        EnemySpawnedEvent?.Invoke();
    }

    public void EnableSpawning()
    {
        InvokeRepeating(
            SPAWN_ENEMY_METHOD,
            waveConfig.GetSpawnDelay(),
            waveConfig.GetSpawnInterval()
        );
    }

    public void DisableSpawning()
    {
        CancelInvoke(SPAWN_ENEMY_METHOD);
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }
}
