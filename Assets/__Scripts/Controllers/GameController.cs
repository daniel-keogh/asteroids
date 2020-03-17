using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public int StartingLives
    {
        get { return startingLives; }
    }

    private int playerScore = 0;
    private int remainingLives;
    [SerializeField] private int startingLives = 3;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Awake()
    {
        SetupSingleton();
    }

    void Start()
    {
        remainingLives = startingLives;

        UpdateScore();
    }

    private void OnEnable()
    {
        Asteroid.EnemyKilledEvent += OnEnemyKilledEvent;
    }

    private void OnDisable()
    {
        Asteroid.EnemyKilledEvent -= OnEnemyKilledEvent;
    }

    private void OnEnemyKilledEvent(Asteroid asteroid)
    {
        // add the score value for the enemy to the player score
        playerScore += asteroid.ScoreValue;
        UpdateScore();
        LoseOneLife();
    }

    private void UpdateScore()
    {
        // Display on screen
        scoreText.text = "Score: " + playerScore.ToString();
    }

    public void LoseOneLife()
    {
        remainingLives--;

        if (remainingLives == 0)
        {
            var sc = FindObjectOfType<SceneController>();
            sc?.GameOver();
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
}
