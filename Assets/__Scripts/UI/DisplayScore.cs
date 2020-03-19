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
        scoreText.text = $"{originalText}{gc.PlayerScore}";
    }
}
