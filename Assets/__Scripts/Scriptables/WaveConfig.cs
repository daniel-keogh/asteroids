using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [Header("Prefabs")]
    [SerializeField] private List<Asteroid> asteroids;
    [SerializeField] private UFO ufo;

    [Header("Spawning")]
    [SerializeField] private float spawnInterval = 2f;
    [SerializeField] private float spawnDelay = 0f;
    [SerializeField] private float waveDelay = 5f;

    [Header("Amount")]
    [SerializeField] private int numAsteroids = 3;
    [SerializeField] private int numUfos = 0;

    public List<Asteroid> GetAsteroids() => asteroids;
    public UFO GetUFO() => ufo;

    public float GetSpawnInterval() => spawnInterval;
    public float GetSpawnDelay() => spawnDelay;
    public float GetWaveDelay() => waveDelay;

    public int GetNumAsteroids() => numAsteroids;
    public int GetNumUFOs() => numUfos;

    public int GetNumEnemies() => numAsteroids + numUfos;

    public Stack<Enemy> CreateEnemyBurst()
    {
        // Returns a Stack containing UFOs and Asteroids shuffled in a random order.
        // The size of the Stack is the sum of numUfos & numAsteroids.

        var enemyBurst = new List<Enemy>();

        for (int i = 0; i < numAsteroids; i++)
        {
            // Pick a random asteroid prefab
            int rIndex = Random.Range(0, asteroids.Count);
            enemyBurst.Add(asteroids[rIndex].GetComponent<Enemy>());
        }

        for (int i = 0; i < numUfos; i++)
        {
            enemyBurst.Add(ufo.GetComponent<Enemy>());
        }

        return ListUtils.CreateShuffledStack(enemyBurst);
    }
}
