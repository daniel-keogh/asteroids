using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;

public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject saveScoreUI;
    [SerializeField] private TMP_InputField inputField;

    private GameController gc;

    private void Start()
    {
        gc = FindObjectOfType<GameController>();

        ToggleSaveScoreUI(false);
    }

    public void ToggleSaveScoreUI(bool show)
    {
        // Show/hide the input popup
        saveScoreUI.SetActive(show);
    }

    public void SaveScore()
    {
        if (gc)
        {
            // Don't accept empty input
            if (inputField.text.Length == 0)
                return;

            // Save the player's score & name to the leaderboard file
            SaveSystem.SaveToLeaderBoard(new PlayerData
            {
                name = inputField.text,
                score = gc.PlayerScore
            });
        }

        // Hide the input box
        ToggleSaveScoreUI(false);
    }
}
