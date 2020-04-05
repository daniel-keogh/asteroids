using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class PointSpawners : MonoBehaviour
{
    [SerializeField] private List<Asteroid> asteroidPrefabs;
    [SerializeField] private float spawnDelay;
    [SerializeField] private float spawnInterval;
    [SerializeField] [Range(-1, 1)] private float xDirection;
    [SerializeField] [Range(-1, 1)] private float yDirection;

    private const string SPAWN_ASTEROID_METHOD = "SpawnOneAsteroid";
    private const string ASTEROID_PARENT = "AsteroidParent";

    private IList<SpawnPoint> spawnPoints;
    private Stack<SpawnPoint> spawnStack;
    private GameObject asteroidParent;

    // Event for telling the system an asteroid has spawned
    public delegate void AsteroidSpawned();
    public static event AsteroidSpawned AsteroidSpawnedEvent;

    void Start()
    {
        asteroidParent = GameObject.Find(ASTEROID_PARENT);

        if (!asteroidParent)
        {
            asteroidParent = new GameObject(ASTEROID_PARENT);
        }

        // Get the spawn points here
        spawnPoints = GetComponentsInChildren<SpawnPoint>();

        // Create the stack of points
        spawnStack = ListUtils.CreateShuffledStack(spawnPoints);

        EnableSpawning();
    }

    private void SpawnOneAsteroid()
    {
        if (spawnStack.Count == 0)
        {
            spawnStack = ListUtils.CreateShuffledStack(spawnPoints);
        }

        var asteriod = Instantiate(
            asteroidPrefabs[Random.Range(0, asteroidPrefabs.Count)],
            asteroidParent.transform
        );

        var sp = spawnStack.Pop();
        asteriod.transform.position = sp.transform.position;

        var movement = asteriod.gameObject.GetComponent<AsteroidMovement>();
        movement.Move(new Vector2(xDirection, yDirection));

        PublishOnEnemySpawnedEvent();
    }

    public void PublishOnEnemySpawnedEvent()
    {
        AsteroidSpawnedEvent?.Invoke();
    }

    public void EnableSpawning()
    {
        InvokeRepeating(SPAWN_ASTEROID_METHOD, spawnDelay, spawnInterval);
    }

    public void DisableSpawning()
    {
        CancelInvoke(SPAWN_ASTEROID_METHOD);
    }
}
