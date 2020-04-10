using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DisplayScore : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private GameController gc;
    private string originalText;

    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        gc = FindObjectOfType<GameController>();

        originalText = scoreText.text;
    }

    void Update()
    {
        string score = "0"; // shown if `gc` is null

        if (gc)
        {
            // Large numbers are separated by commas
            score = string.Format("{0:n0}", gc.PlayerScore);
        }

        scoreText.text = $"{originalText}{score}";
    }
}
