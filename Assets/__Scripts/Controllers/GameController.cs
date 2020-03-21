using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private int playerScore = 0;
    private int remainingLives;
    private int remainingAsteroids;
    private int waveNumber;

    [SerializeField] private int startingLives = 3;
    [SerializeField] private int asteroidCountPerWave;

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
        Asteroid.AsteroidDestroyedEvent += OnAsteroidDestroyedEvent;
        Player.PlayerKilledEvent += OnPlayerKilledEvent;
        PointSpawners.EnemySpawnedEvent += OnEnemySpawnedEvent;
    }

    private void OnDisable()
    {
        Asteroid.AsteroidDestroyedEvent -= OnAsteroidDestroyedEvent;
        Player.PlayerKilledEvent -= OnPlayerKilledEvent;
        PointSpawners.EnemySpawnedEvent -= OnEnemySpawnedEvent;
    }

    private void OnEnemySpawnedEvent()
    {
        remainingAsteroids--;

        if (remainingAsteroids == 0)
        {
            DisableSpawning();
        }
    }

    private IEnumerator SetupNextWave()
    {
        yield return new WaitForSeconds(5.0f);

        waveNumber++; // not displayed
        remainingAsteroids = asteroidCountPerWave;
        EnableSpawning();
    }


    private void OnAsteroidDestroyedEvent(Asteroid asteroid)
    {
        if (asteroid != null)
        {
            // add the score value for the enemy to the player score
            playerScore += asteroid.ScoreValue;
        }

        int currentWaveSize = FindObjectsOfType<Asteroid>().Length - 1;

        if (currentWaveSize == 0)
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
