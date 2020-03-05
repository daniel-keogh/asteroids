using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    [SerializeField] private LifeIcon lifeIconPrefab;
    private int startingLives; // read from the GameController
    private GameController gc;

    void Start()
    {
        // Get the GameController object
        gc = FindObjectOfType<GameController>();

        if (gc)
        {
            // Retrieve the starting lives value
            startingLives = gc.StartingLives;

            CreateIcons();
        }
    }

    private void CreateIcons()
    {
        // Show the appropriate number of hearts on the screen
        for (int i = 0; i < startingLives; i++)
        {
            LifeIcon icon = Instantiate(lifeIconPrefab, transform);
        }
    }
}
