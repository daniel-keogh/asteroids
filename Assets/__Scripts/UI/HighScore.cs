using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HighScore : MonoBehaviour
{
    [SerializeField] [TextArea] private string newHighScoreMsg = "New High Score!";
    [SerializeField] private Color scoreColour = Color.yellow;

    private TextMeshProUGUI highScoreText;
    private int currentScore;
    private GameController gc;

    private const string HIGH_SCORE_PREF = "HighScore";

    void Start()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
        gc = FindObjectOfType<GameController>();

        if (gc)
        {
            int highScore = GetHighScore();

            if (gc.PlayerScore > highScore)
            {
                SaveNewHighScore();
                highScoreText.text = $"{newHighScoreMsg}\n\n{gc.PlayerScore}";
            }
            else
            {
                highScoreText.text = GetNewHighScoreStr(highScore);
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

    private string GetNewHighScoreStr(int highScore)
    {
        var colour = ColorUtility.ToHtmlStringRGBA(scoreColour);
        return $"<#{colour}><b>{gc.PlayerScore}<b></color>\n\nHigh Score: {highScore}";
    }
}
