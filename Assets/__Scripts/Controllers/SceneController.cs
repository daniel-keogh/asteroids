using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneController : MonoBehaviour
{
    [Header("Animate Scene Transitions")]
    [SerializeField] private float transitionDelay = 1f;
    [SerializeField] private Animator transitionAnimator;

    // Animation triggers
    private const string ANIMATOR_TRIGGER = "Start";

    public void PlayOnClick()
    {
        // Reset the GameController singleton before re-playing.
        FindObjectOfType<GameController>()?.ResetGame();
        ChangeScene(SceneNames.GAME_SCENE);
    }

    public void GoToMainMenu() => ChangeScene(SceneNames.MAIN_MENU);
    public void GoToOptionsMenu() => ChangeScene(SceneNames.OPTIONS_MENU);
    public void GoToLeaderBoard() => ChangeScene(SceneNames.LEADERBOARD_SCREEN);
    public void GoToTutorial() => ChangeScene(SceneNames.TUTORIAL);
    public void GameOver() => ChangeScene(SceneNames.GAME_OVER);
    public void QuitOnClick() => Application.Quit();

    public void ChangeScene(string name)
    {
        StartCoroutine(SceneTransition(name));
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
