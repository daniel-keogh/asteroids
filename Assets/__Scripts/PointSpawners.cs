using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PointSpawners : MonoBehaviour
{
    // Event for telling the system an asteroid has spawned
    public delegate void EnemySpawned();
    public static event EnemySpawned EnemySpawnedEvent;

    [SerializeField] private Asteroid asteroidPrefab;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float spawnInterval;
    [SerializeField] [Range(-1, 1)] private float xDirection;
    [SerializeField] [Range(-1, 1)] private float yDirection;

    private const string SPAWN_ENEMY_METHOD = "SpawnOneEnemy";
    private const string ASTEROID_PARENT = "AsteroidParent";

    private IList<SpawnPoint> spawnPoints;
    private Stack<SpawnPoint> spawnStack;
    private GameObject enemyParent;

    void Start()
    {
        enemyParent = GameObject.Find(ASTEROID_PARENT);

        if (!enemyParent)
        {
            enemyParent = new GameObject(ASTEROID_PARENT);
        }

        // Get the spawn points here
        spawnPoints = GetComponentsInChildren<SpawnPoint>();

        SpawnEnemyWaves();
        EnableSpawning();
    }

    private void SpawnEnemyWaves()
    {
        // Create the stack of points
        spawnStack = ListUtils.CreateShuffledStack(spawnPoints);

        InvokeRepeating(SPAWN_ENEMY_METHOD, spawnDelay, spawnInterval);
    }

    private void SpawnOneEnemy()
    {
        if (spawnStack.Count == 0)
        {
            spawnStack = ListUtils.CreateShuffledStack(spawnPoints);
        }

        var sp = spawnStack.Pop();

        var asteriod = Instantiate(asteroidPrefab, enemyParent.transform);
        asteriod.transform.position = sp.transform.position;

        var movement = asteriod.gameObject.GetComponent<AsteroidMovement>();
        movement.Move(new Vector2(xDirection, yDirection));

        PublishOnEnemySpawnedEvent();
    }

    public void PublishOnEnemySpawnedEvent()
    {
        EnemySpawnedEvent?.Invoke();
    }

    public void EnableSpawning()
    {
        InvokeRepeating(SPAWN_ENEMY_METHOD, spawnDelay, spawnInterval);
    }

    public void DisableSpawning()
    {
        CancelInvoke(SPAWN_ENEMY_METHOD);
    }
}
