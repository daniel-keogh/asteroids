using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneController : MonoBehaviour
{
    [SerializeField] private float gameOverDelay;

    public void PlayOnClick()
    {
        // Reset the GameController singleton before re-playing.
        FindObjectOfType<GameController>()?.ResetGame();

        SceneManager.LoadSceneAsync(SceneNames.GAME_SCENE);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(SceneNames.MAIN_MENU);
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    public void QuitOnClick()
    {
        // UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(gameOverDelay);

        SceneManager.LoadSceneAsync(SceneNames.GAME_OVER);
    }
}
