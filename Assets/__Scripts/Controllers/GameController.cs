using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    private int playerScore = 0;
    [SerializeField] private TextMeshProUGUI scoreText;

    void Awake()
    {
        SetupSingleton();
    }

    void Start()
    {
        UpdateScore();
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

    private void UpdateScore()
    {
        // Display on screen
        scoreText.text = playerScore.ToString();
    }
}
