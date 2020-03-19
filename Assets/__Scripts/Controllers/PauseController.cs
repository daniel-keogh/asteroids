using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;

    private KeyCode pauseKey = KeyCode.Escape;
    private bool IsPaused = false;

    void Start()
    {
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            if (IsPaused)
            {
                SetPaused(false);
            }
            else
            {
                SetPaused(true);
            }
        }
    }

    public void SetPaused(bool status)
    {
        Time.timeScale = status ? 0 : 1; // stop/start time
        pauseMenu.SetActive(status); // show the pause menu UI
        IsPaused = status;
    }

    public void QuitToMainMenu()
    {
        var sc = FindObjectOfType<SceneController>();

        if (sc)
        {
            Time.timeScale = 1;
            sc.GoToMainMenu();
        }
    }
}
