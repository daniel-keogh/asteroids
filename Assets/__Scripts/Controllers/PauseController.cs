using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioClip onPauseClip;

    private KeyCode pauseKey = KeyCode.Escape;
    private bool IsPaused = false;
    private SoundController soundController;

    void Start()
    {
        pauseMenu.SetActive(false);
        soundController = FindObjectOfType<SoundController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            // Pause/Unpause
            SetPaused(!IsPaused);
        }
    }

    public void SetPaused(bool status)
    {
        if (status && onPauseClip)
        {
            soundController?.PlayOneShot(onPauseClip);
        }

        Time.timeScale = status ? 0 : 1; // stop/start time
        pauseMenu.SetActive(status); // show the pause menu UI
        IsPaused = status;
    }

    public void QuitToMainMenu()
    {
        var sc = FindObjectOfType<SceneController>();

        if (sc)
        {
            Time.timeScale = 1; // Reset time
            sc.GoToMainMenu();
        }
    }
}
