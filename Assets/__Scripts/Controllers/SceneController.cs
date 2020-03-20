using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneController : MonoBehaviour
{
    [SerializeField] private float sceneTransitionDelay;
    [SerializeField] private Animator crossFadeAnimator;

    private const string ANIMATOR_TRIGGER = "Start";

    public void PlayOnClick()
    {
        // Reset the GameController singleton before re-playing.
        FindObjectOfType<GameController>()?.ResetGame();

        StartCoroutine(SceneTransition(SceneNames.GAME_SCENE, ANIMATOR_TRIGGER));
    }

    public void GoToMainMenu()
    {
        StartCoroutine(SceneTransition(SceneNames.MAIN_MENU, ANIMATOR_TRIGGER));
    }

    public void GameOver()
    {
        StartCoroutine(SceneTransition(SceneNames.GAME_OVER, ANIMATOR_TRIGGER));
    }

    public void QuitOnClick()
    {
        // UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }

    private IEnumerator SceneTransition(string sceneName, string trigger)
    {
        crossFadeAnimator.SetTrigger(trigger);

        yield return new WaitForSeconds(sceneTransitionDelay);

        SceneManager.LoadSceneAsync(sceneName);
    }
}
