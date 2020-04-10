using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy Wave Config")]
public class WaveConfig : ScriptableObject
{
    [Header("Prefabs")]
    [SerializeField] private List<Enemy> enemies;

    [Header("Spawning")]
    [SerializeField] private float spawnInterval = 3f;
    [SerializeField] private float spawnDelay = 0f;
    [SerializeField] private float delayPerWave = 5f;

    [Header("Amount")]
    [SerializeField] private int numAsteroidsPerWave = 3;
    [SerializeField] private int numUFOsPerWave = 1;

    public List<Enemy> GetEnemies() { return enemies; }

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
}
