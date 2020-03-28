using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HighScore : MonoBehaviour
{
    [SerializeField] [TextArea] private string newHighScoreMsg = "New High Score!";

    private TextMeshProUGUI highScoreText;
    private GameController gc;

    private const string HIGH_SCORE_PREF = "HighScore";

    void Start()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
        gc = FindObjectOfType<GameController>();

        int highScore = GetHighScore();

        if (gc)
        {
            if (gc.PlayerScore > highScore)
            {
                SaveNewHighScore();
                highScoreText.text = newHighScoreMsg;
            }
            else
            {
                highScoreText.text = highScore.ToString();
            }
        }
    }

    public void SaveNewHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_PREF, gc.PlayerScore);
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_PREF, 0);
    }
}
