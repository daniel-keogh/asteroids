using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class OptionsMenu : MonoBehaviour
{
    public void ToggleMusic()
    {
        MusicPlayer.FindMusicPlayer()?.ToggleMusic();
    }

    public void ToggleSoundEffects()
    {
        SoundController.FindSoundController()?.ToggleSounds();
    }

    public void ClearLeaderBoard()
    {
        SaveSystem.ClearLeaderBoard();
    }
}
