using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        SetupSingleton();
    }

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void ToggleMusic()
    {
        audioSource.mute = !audioSource.mute;
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
