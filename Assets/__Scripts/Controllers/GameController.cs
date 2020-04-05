﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public int StartingLives
    {
        get { return startingLives; }
    }

    public int RemainingLives
    {
        get { return remainingLives; }
    }

    public int PlayerScore
    {
        get { return playerScore; }
    }

    [Header("Player Lives")]
    [SerializeField] private int startingLives = 3;

    [Header("Waves")]
    [SerializeField] private int asteroidCountPerWave = 3;
    [SerializeField] private float delayPerWave = 5f;
    [SerializeField] private TextMeshProUGUI waveIndicator;

    private int playerScore = 0;
    private int waveNumber = 1;
    private int remainingLives;
    private int remainingAsteroids;

    void Awake()
    {
        SetupSingleton();
    }

    void Start()
    {
        remainingLives = startingLives;
        remainingAsteroids = asteroidCountPerWave;
    }

    private void OnEnable()
    {
        Enemy.EnemyDestroyedEvent += OnEnemyDestroyedEvent;
        Player.PlayerKilledEvent += OnPlayerKilledEvent;
        PointSpawners.AsteroidSpawnedEvent += OnAsteroidSpawnedEvent;
    }

    private void OnDisable()
    {
        Enemy.EnemyDestroyedEvent -= OnEnemyDestroyedEvent;
        Player.PlayerKilledEvent -= OnPlayerKilledEvent;
        PointSpawners.AsteroidSpawnedEvent -= OnAsteroidSpawnedEvent;
    }

    private void OnAsteroidSpawnedEvent()
    {
        remainingAsteroids--;

        if (remainingAsteroids == 0)
        {
            DisableSpawning();
        }
    }

    private IEnumerator SetupNextWave()
    {
        waveIndicator.gameObject.SetActive(true);
        // show some feedback to the player
        waveIndicator.text = $"Wave {++waveNumber}";

        yield return new WaitForSeconds(delayPerWave);

        waveIndicator.gameObject.SetActive(false);

        remainingAsteroids = asteroidCountPerWave;
        EnableSpawning();
    }

    private void OnEnemyDestroyedEvent(Enemy enemy)
    {
        // add the score value to the player score
        playerScore += enemy.ScoreValue;

        int currentWaveSize = FindObjectsOfType<Enemy>().Length - 1;

        if (currentWaveSize == 0 && remainingAsteroids == 0)
        {
            StartCoroutine(SetupNextWave());
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

    public void DisableSpawning()
    {
        foreach (var spawner in FindObjectsOfType<PointSpawners>())
        {
            spawner.DisableSpawning();
        }
    }

    public void EnableSpawning()
    {
        foreach (var spawner in FindObjectsOfType<PointSpawners>())
        {
            spawner.EnableSpawning();
        }
    }

    private void SetupSingleton()
    {
        // Check for any other objects of the same type
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject); // destroy the current object
        }
        else
        {
            DontDestroyOnLoad(gameObject); // persist across scenes
        }
    }

    public void ResetGame()
    {
        Destroy(gameObject);
    }
}
