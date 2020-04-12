using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    private const string MUTE_PREF = "MUTE_MUSICPLAYER";

    void Awake()
    {
        SetupSingleton();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Set whether MusicPlayer should be muted or not by reading PlayerPrefs
        int flag = PlayerPrefs.GetInt(MUTE_PREF, 0);
        audioSource.mute = flag == 1;
    }

    private void SetupSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject); // Destroy the current object
        }
        else
        {
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
    }

    public void ToggleMusic()
    {
        // Save player's preference
        audioSource.mute = !audioSource.mute;
        PlayerPrefs.SetInt(MUTE_PREF, audioSource.mute ? 1 : 0);
    }

    public bool IsMuted()
    {
        return audioSource.mute;
    }

    public static MusicPlayer FindMusicPlayer()
    {
        MusicPlayer mp = FindObjectOfType<MusicPlayer>();

        if (!mp)
        {
            Debug.LogWarning("Missing MusicPlayer");
        }

        return mp;
    }
}
