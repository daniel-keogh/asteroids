﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Utilities;

public class GameController : SingletonController
{
    [Header("Player Lives")]
    [SerializeField] private int startingLives = 3;
    [SerializeField] private int pointsForExtraLife = 1600;

    [Header("Waves")]
    [SerializeField] private List<WaveConfig> waveConfigs;
    [SerializeField] private TextMeshProUGUI waveIndicator;

    private int playerScore = 0;
    private int waveNumber = 1;
    private int currentWaveIndex = 0;
    private int remainingLives;
    private int remainingEnemies;
    private Coroutine nextWaveCoroutine;

    public int StartingLives => startingLives;
    public int RemainingLives => remainingLives;
    public int PlayerScore => playerScore;
    public int WaveNumber => waveNumber;

    void Start()
    {
        remainingLives = startingLives;

        // Only start the wave Coroutine if in the GameScene
        if (SceneManager.GetActiveScene().name == SceneNames.GAME_SCENE)
        {
            nextWaveCoroutine = StartCoroutine(SetupNextWave(waveConfigs[currentWaveIndex]));
        }
        else
        {
            // If no longer in GameScene, make sure to stop the wave Coroutine
            if (nextWaveCoroutine != null)
            {
                StopCoroutine(nextWaveCoroutine);
            }
        }
    }

    private void OnEnable()
    {
        Enemy.EnemyDestroyedEvent += OnEnemyDestroyedEvent;
        Player.PlayerKilledEvent += OnPlayerKilledEvent;
        PointSpawners.EnemySpawnedEvent += OnEnemySpawnedEvent;
    }

    private void OnDisable()
    {
        Enemy.EnemyDestroyedEvent -= OnEnemyDestroyedEvent;
        Player.PlayerKilledEvent -= OnPlayerKilledEvent;
        PointSpawners.EnemySpawnedEvent -= OnEnemySpawnedEvent;
    }

    private IEnumerator SetupNextWave(WaveConfig currentWave)
    {
        // Don't show indicator on the first wave
        if (waveNumber == 1)
        {
            yield return new WaitForSeconds(currentWave.GetWaveDelay());
            waveNumber++;
        }
        else
        {
            // Show some feedback to the player
            waveIndicator.gameObject.SetActive(true);
            waveIndicator.text = $"Level {waveNumber++}";

            yield return new WaitForSeconds(currentWave.GetWaveDelay());

            waveIndicator.gameObject.SetActive(false);
        }

        // The number of enemies in this wave
        remainingEnemies = currentWave.GetNumEnemies();

        // Pass the config file to the PointSpawner
        FindObjectOfType<PointSpawners>()?.SetWaveConfig(currentWave);

        // Start spawning enemies
        EnableSpawning();
    }

    private WaveConfig NextWave()
    {
        if (currentWaveIndex < waveConfigs.Count - 1)
        {
            // Return the next WaveConfig
            currentWaveIndex++;
        }
        else
        {
            // Keep returning the last WaveConfig
            return waveConfigs[waveConfigs.Count - 1];
        }

        return waveConfigs[currentWaveIndex];
    }

    private void OnEnemySpawnedEvent()
    {
        remainingEnemies--;

        if (remainingEnemies == 0)
        {
            DisableSpawning();
        }
    }

    private void OnEnemyDestroyedEvent(Enemy enemy)
    {
        // Add the score value to the player score
        playerScore += enemy.ScoreValue;

        if (playerScore >= pointsForExtraLife)
        {
            // Give player an extra life
            FindObjectOfType<LifeCounter>()?.AwardExtraLife();
            remainingLives++;
            pointsForExtraLife += playerScore;
        }

        // Determine if its time for another wave
        int currentWaveSize = FindObjectsOfType<Enemy>().Length - 1;

        if (currentWaveSize == 0 && remainingEnemies == 0)
        {
            nextWaveCoroutine = StartCoroutine(SetupNextWave(NextWave()));
        }
    }

    private void OnPlayerKilledEvent()
    {
        remainingLives--;

        if (remainingLives == 0)
        {
            FindObjectOfType<SceneController>()?.GameOver();
        }
    }

    private void EnableSpawning()
    {
        foreach (var spawner in FindObjectsOfType<PointSpawners>())
        {
            spawner.EnableSpawning();
        }
    }

    private void DisableSpawning()
    {
        foreach (var spawner in FindObjectsOfType<PointSpawners>())
        {
            spawner.DisableSpawning();
        }
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
