using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounter : MonoBehaviour
{
    [SerializeField] private LifeIcon lifeIconPrefab;
    [SerializeField] private AudioClip extraLifeGivenClip;

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
            ShowFinalLifeAnimation(true);
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
            ShowFinalLifeAnimation(true);
        }
    }

    private void ShowFinalLifeAnimation(bool show)
    {
        // Enable/disable animation for the icon on the top of the stack
        lifeIcons.Peek().SetIsFinalLife(show);
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

    public void AwardExtraLife()
    {
        // Disable animation if necessary
        if (lifeIcons.Count == 1)
        {
            ShowFinalLifeAnimation(false);
        }

        // Play a sound
        SoundController.FindSoundController()?.PlayOneShot(extraLifeGivenClip);

        // Add an icon to the stack
        lifeIcons.Push(Instantiate(lifeIconPrefab, transform));
    }
}
