using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    [SerializeField] private LifeIcon lifeIconPrefab;

    private int remainingLives;
    private GameController gc;
    private Stack<LifeIcon> lifeIcons;

    void Start()
    {
        // Get the GameController object
        gc = FindObjectOfType<GameController>();

        if (gc)
        {
            // Retrieve the starting lives value
            remainingLives = gc.StartingLives;
            lifeIcons = new Stack<LifeIcon>(remainingLives);

            CreateIcons();
        }
    }

    void Update()
    {
        CheckLivesRemaining();
    }

    private void CheckLivesRemaining()
    {
        if (gc)
        {
            if (remainingLives != gc.RemainingLives)
            {
                Destroy(lifeIcons.Pop().gameObject);
                remainingLives = gc.RemainingLives;
            }
        }
    }

    private void CreateIcons()
    {
        // Show the appropriate number of hearts on the screen
        for (int i = 0; i < remainingLives; i++)
        {
            lifeIcons.Push(Instantiate(lifeIconPrefab, transform));
        }
    }
}
