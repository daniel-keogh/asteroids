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
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        IsPaused = true;
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        IsPaused = false;
    }

    public void QuitToMainMenu()
    {
        var sc = FindObjectOfType<SceneController>();
        var gc = FindObjectOfType<GameController>();

        if (sc && gc)
        {
            Time.timeScale = 1;
            sc.GoToMainMenu();

            Destroy(gc);
        }
    }
}
