using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class OptionsMenu : MonoBehaviour
{
    private Toggle musicToggle;
    private MusicPlayer musicPlayer;

    void Start()
    {
        musicToggle = GetComponentInChildren<Toggle>();

        musicPlayer = MusicPlayer.FindMusicPlayer();

        // Set the toggle's default value
        if (musicPlayer)
        {
            musicToggle.SetIsOnWithoutNotify(!musicPlayer.IsMuted());
        }
    }

    public void ToggleMusic()
    {
        musicPlayer?.ToggleMusic();
    }

    public void ClearLeaderBoard()
    {
        SaveSystem.ClearLeaderBoard();
    }
}
