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

    [SerializeField] private int startingLives = 3;

    void Awake()
    {
        SetupSingleton();
    }

    void Start()
    {
        remainingLives = startingLives;
    }

    private void OnEnable()
    {
        Asteroid.AsteroidDestroyedEvent += OnAsteroidDestroyedEvent;
        Player.PlayerKilledEvent += OnPlayerKilledEvent;
    }

    private void OnDisable()
    {
        Asteroid.AsteroidDestroyedEvent -= OnAsteroidDestroyedEvent;
        Player.PlayerKilledEvent -= OnPlayerKilledEvent;
    }

    private void OnAsteroidDestroyedEvent(Asteroid asteroid)
    {
        // add the score value for the enemy to the player score
        playerScore += asteroid.ScoreValue;
    }

    private void OnPlayerKilledEvent(Player player)
    {
        LoseOneLife();
    }

    public void LoseOneLife()
    {
        remainingLives--;

        if (remainingLives == 0)
        {
            FindObjectOfType<SceneController>()?.GameOver();
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
