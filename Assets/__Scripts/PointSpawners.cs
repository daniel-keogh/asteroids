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
    private Stack<Enemy> burst = new Stack<Enemy>();

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

        // Get the SpawnPoints
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

        if (burst.Count == 0)
        {
            // Get a Stack of the next round of enemies to be spawned
            burst = waveConfig.CreateEnemyBurst();
        }

        // Position the new Enemy
        var enemy = Instantiate(burst.Pop(), enemyParent.transform);
        var sp = spawnStack.Pop();
        enemy.transform.position = sp.transform.position;

        // Get the Asteroids moving
        var asteroid = enemy.GetComponent<AsteroidMovement>();
        if (asteroid)
        {
            SetAsteroidMovement(sp, asteroid);
        }

        PublishOnEnemySpawnedEvent();
    }

    private void SetAsteroidMovement(SpawnPoint sp, AsteroidMovement asteroid)
    {
        float xDirection, yDirection;

        // Determine movement direction based on the position of the SpawnPoint
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

        // Move in that direction
        asteroid.Move(new Vector2(xDirection, yDirection));
    }

    private void PublishOnEnemySpawnedEvent()
    {
        // Signal that an Enemy has spawned
        EnemySpawnedEvent?.Invoke();
    }

    public void SetWaveConfig(WaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
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
}
