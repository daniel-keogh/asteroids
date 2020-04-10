using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    [SerializeField] private LifeIcon lifeIconPrefab;

    private GameController gc;
    private Stack<LifeIcon> lifeIcons;

    void Start()
    {
        // Get the GameController object
        gc = FindObjectOfType<GameController>();

        if (gc)
        {
            SetupLifeIcons();
        }

        if (lifeIcons.Count == 1)
        {
            ShowFinalLifeAnimation();
        }
    }

    private void OnEnable()
    {
        Player.PlayerKilledEvent += OnPlayerKilledEvent;
    }

    private void OnDisable()
    {
        Player.PlayerKilledEvent -= OnPlayerKilledEvent;
    }

    private void OnPlayerKilledEvent()
    {
        if (lifeIcons.Count > 0)
        {
            // Delete one of the LifeIcons
            Destroy(lifeIcons.Pop().gameObject);
        }

        if (lifeIcons.Count == 1)
        {
            // Indicate this is the Player's final life
            ShowFinalLifeAnimation();
        }
    }

    private void ShowFinalLifeAnimation()
    {
        // Animate the icon on the top of the stack
        lifeIcons.Peek().SetAnimatorEnabled(true);

        // TODO: play a sound
    }

    private void SetupLifeIcons()
    {
        if (gc)
        {
            lifeIcons = new Stack<LifeIcon>(gc.StartingLives);

            // Show the appropriate number of hearts on the screen
            for (int i = 0; i < gc.StartingLives; i++)
            {
                lifeIcons.Push(Instantiate(lifeIconPrefab, transform));
            }
        }
    }
}
