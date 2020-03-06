using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class SceneController : MonoBehaviour
{
    public void PlayOnClick()
    {
        SceneManager.LoadSceneAsync(SceneNames.GAME_SCENE);
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(SceneNames.MAIN_MENU);
    }

    public void QuitOnClick()
    {
        // UnityEditor.EditorApplication.isPlaying = false;

        Application.Quit();
    }
}
