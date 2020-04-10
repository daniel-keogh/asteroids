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
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnDelay = 0f;
    [SerializeField] private float delayPerWave = 5f;

    [Header("Amount")]
    [SerializeField] private int numAsteroidsPerWave = 3;
    [SerializeField] private int numUFOsPerWave = 1;

    public List<Asteroid> GetAsteroids() { return asteroids; }
    public UFO GetUFO() { return ufo; }

    public float GetSpawnInterval() { return spawnInterval; }
    public float GetSpawnDelay() { return spawnDelay; }
    public float GetDelayPerWave() { return delayPerWave; }

    public int GetNumAsteroidsPerWave() { return numAsteroidsPerWave; }
    public int GetNumUFOsPerWave() { return numUFOsPerWave; }

    public int GetNumEnemiesPerWave() { return numAsteroidsPerWave + numUFOsPerWave; }

    public void SetNumAsteroidsPerWave(int numAsteroidsPerWave)
    {
        this.numAsteroidsPerWave = numAsteroidsPerWave;
    }

    public void SetNumUFOsPerWave(int numUFOsPerWave)
    {
        this.numUFOsPerWave = numUFOsPerWave;
    }

    public Stack<Enemy> CreateEnemyBurst()
    {
        var enemyBurst = new List<Enemy>();

        for (int i = 0; i < numAsteroidsPerWave; i++)
        {
            int rIndex = Random.Range(0, asteroids.Count);
            enemyBurst.Add(asteroids[rIndex].GetComponent<Enemy>());
        }

        for (int i = 0; i < numUFOsPerWave; i++)
        {
            enemyBurst.Add(ufo.GetComponent<Enemy>());
        }

        return ListUtils.CreateShuffledStack(enemyBurst);
    }
}
