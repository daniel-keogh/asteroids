using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Data;
using Utilities;

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
    [SerializeField] private List<WaveConfig> waveConfigs;
    [SerializeField] private TextMeshProUGUI waveIndicator;

    [Header("LeaderBoard")]
    [SerializeField] private int maxLeaderBoardSize = 12;

    private int playerScore = 0;
    private int waveNumber = 1;
    private int remainingLives;
    private int remainingEnemies;
    private int currentWaveIndex = 0;

    void Awake()
    {
        SetupSingleton();
    }

    void Start()
    {
        SaveSystem.MaxLeaderBoardSize = maxLeaderBoardSize;

        remainingLives = startingLives;

        if (SceneManager.GetActiveScene().name == SceneNames.GAME_SCENE)
        {
            StartCoroutine(SetupNextWave(waveConfigs[currentWaveIndex]));
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
            waveIndicator.text = $"Wave {waveNumber++}";

            yield return new WaitForSeconds(currentWave.GetWaveDelay());

            waveIndicator.gameObject.SetActive(false);
        }

        remainingEnemies = currentWave.GetNumEnemies();

        // Pass the config file to the PointSpawner
        FindObjectOfType<PointSpawners>().SetWaveConfig(currentWave);

        EnableSpawning();
    }

    private WaveConfig NextWave()
    {
        if (currentWaveIndex < waveConfigs.Count - 1)
        {
            currentWaveIndex++;
        }
        else
        {
            // return the last WaveConfig
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
        // add the score value to the player score
        playerScore += enemy.ScoreValue;

        int currentWaveSize = FindObjectsOfType<Enemy>().Length - 1;

        if (currentWaveSize == 0 && remainingEnemies == 0)
        {
            StartCoroutine(SetupNextWave(NextWave()));
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

    public void EnableSpawning()
    {
        foreach (var spawner in FindObjectsOfType<PointSpawners>())
        {
            spawner.EnableSpawning();
        }
    }

    public void DisableSpawning()
    {
        foreach (var spawner in FindObjectsOfType<PointSpawners>())
        {
            spawner.DisableSpawning();
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
