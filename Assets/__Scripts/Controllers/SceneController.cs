using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneController : MonoBehaviour
{
    [SerializeField] private float transitionDelay;
    [SerializeField] private Animator transitionAnimator;

    private const string ANIMATOR_TRIGGER = "Start";

    public void PlayOnClick()
    {
        // Reset the GameController singleton before re-playing.
        FindObjectOfType<GameController>()?.ResetGame();

        StartCoroutine(SceneTransition(SceneNames.GAME_SCENE));
    }

    public void GoToMainMenu()
    {
        StartCoroutine(SceneTransition(SceneNames.MAIN_MENU));
    }

    public void GoToOptionsMenu()
    {
        StartCoroutine(SceneTransition(SceneNames.OPTIONS_MENU));
    }

    public void GoToLeaderBoard()
    {
        StartCoroutine(SceneTransition(SceneNames.LEADERBOARD_SCREEN));
    }

    public void GameOver()
    {
        StartCoroutine(SceneTransition(SceneNames.GAME_OVER));
    }

    public void QuitOnClick()
    {
        // UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    private IEnumerator SceneTransition(string sceneName)
    {
        if (transitionAnimator)
        {
            // Show an animation
            transitionAnimator.SetTrigger(ANIMATOR_TRIGGER);
        }

        yield return new WaitForSeconds(transitionDelay);

        SceneManager.LoadSceneAsync(sceneName);
    }
}
