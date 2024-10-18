using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;
    public AudioClip backgroundMusic;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            PlayBackgroundMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void PlayBackgroundMusic()
    {
        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.clip = backgroundMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("AudioSource or BackgroundMusic is not assigned.");
        }
    }
}
