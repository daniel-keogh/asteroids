using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;

public class GaveOver : MonoBehaviour
{
    [SerializeField] private GameObject saveScoreUI;
    [SerializeField] private TMP_InputField inputField;

    private GameController gc;

    private void Start()
    {
        saveScoreUI.SetActive(false);

        gc = FindObjectOfType<GameController>();
    }

    public void ToggleSaveScoreUI(bool show)
    {
        saveScoreUI.SetActive(show);
    }

    public void SaveScore()
    {
        if (gc)
        {
            if (inputField.text.Length > 0)
                return;

            SaveSystem.SaveToLeaderBoard(new PlayerData
            {
                name = inputField.text,
                score = gc.PlayerScore
            });
        }

        saveScoreUI.SetActive(false);
    }
}
